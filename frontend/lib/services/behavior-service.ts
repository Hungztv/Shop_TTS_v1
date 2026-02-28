import axios from 'axios';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5266/api';

// ══════════════════════════════════════════
//  Types
// ══════════════════════════════════════════

export enum BehaviorType {
  View = 0,
  Search = 1,
  AddToCart = 2,
  Purchase = 3,
  Wishlist = 4,
  Rating = 5,
  Compare = 6,
}

export interface TrackEvent {
  sessionId?: string;
  behaviorType: BehaviorType;
  productId?: number;
  searchQuery?: string;
  ratingScore?: number;
  dwellTimeSeconds?: number;
  sourcePage?: string;
}

export interface RecommendedProduct {
  id: number;
  name: string;
  slug: string;
  price: number;
  image: string;
  brandName: string;
  categoryName: string;
  averageScore: number;
  ratingCount: number;
  soldOut: number;
  isInStock: boolean;
}

// ══════════════════════════════════════════
//  Session ID management
// ══════════════════════════════════════════

const SESSION_KEY = 'shoptts_session_id';

export function getSessionId(): string {
  if (typeof window === 'undefined') return '';
  let sessionId = sessionStorage.getItem(SESSION_KEY);
  if (!sessionId) {
    sessionId = `sess_${Date.now()}_${Math.random().toString(36).substring(2, 9)}`;
    sessionStorage.setItem(SESSION_KEY, sessionId);
  }
  return sessionId;
}

// ══════════════════════════════════════════
//  Auth token helper
// ══════════════════════════════════════════

function getAuthHeaders(): Record<string, string> {
  if (typeof window === 'undefined') return {};
  const token = localStorage.getItem('token') || localStorage.getItem('access_token');
  if (token) {
    return { Authorization: `Bearer ${token}` };
  }
  return {};
}

// ══════════════════════════════════════════
//  Tracking functions
// ══════════════════════════════════════════

// Queue for batching events
let eventQueue: TrackEvent[] = [];
let flushTimer: ReturnType<typeof setTimeout> | null = null;
const FLUSH_INTERVAL = 5000; // 5 seconds
const MAX_BATCH_SIZE = 20;

/**
 * Track a single behavior event. Events are batched and sent every 5s.
 */
export function trackBehavior(event: Omit<TrackEvent, 'sessionId'>): void {
  const fullEvent: TrackEvent = {
    ...event,
    sessionId: getSessionId(),
  };

  eventQueue.push(fullEvent);

  // Flush immediately if batch is full
  if (eventQueue.length >= MAX_BATCH_SIZE) {
    flushEvents();
    return;
  }

  // Schedule flush
  if (!flushTimer) {
    flushTimer = setTimeout(flushEvents, FLUSH_INTERVAL);
  }
}

/**
 * Flush all queued events to the API
 */
async function flushEvents(): Promise<void> {
  if (flushTimer) {
    clearTimeout(flushTimer);
    flushTimer = null;
  }

  if (eventQueue.length === 0) return;

  const eventsToSend = [...eventQueue];
  eventQueue = [];

  try {
    if (eventsToSend.length === 1) {
      await axios.post(`${API_URL}/behavior/track`, eventsToSend[0], {
        headers: getAuthHeaders(),
      });
    } else {
      await axios.post(
        `${API_URL}/behavior/track-batch`,
        {
          sessionId: getSessionId(),
          events: eventsToSend,
        },
        { headers: getAuthHeaders() }
      );
    }
  } catch (error) {
    // Silently fail — tracking should never block UX
    console.warn('[Behavior] Failed to send tracking events:', error);
  }
}

// Flush on page unload
if (typeof window !== 'undefined') {
  window.addEventListener('beforeunload', () => {
    if (eventQueue.length > 0) {
      const body = JSON.stringify(
        eventQueue.length === 1
          ? eventQueue[0]
          : { sessionId: getSessionId(), events: eventQueue }
      );
      const url =
        eventQueue.length === 1
          ? `${API_URL}/behavior/track`
          : `${API_URL}/behavior/track-batch`;

      // Use sendBeacon for reliable delivery on page unload
      navigator.sendBeacon(url, new Blob([body], { type: 'application/json' }));
      eventQueue = [];
    }
  });
}

// ══════════════════════════════════════════
//  Convenience tracking helpers
// ══════════════════════════════════════════

/** Track a product page view */
export function trackProductView(productId: number, sourcePage?: string): void {
  trackBehavior({
    behaviorType: BehaviorType.View,
    productId,
    sourcePage: sourcePage || window?.location?.pathname,
  });
}

/** Track a search query */
export function trackSearch(query: string, sourcePage?: string): void {
  trackBehavior({
    behaviorType: BehaviorType.Search,
    searchQuery: query,
    sourcePage: sourcePage || 'search',
  });
}

/** Track an add-to-cart action */
export function trackAddToCart(productId: number, sourcePage?: string): void {
  trackBehavior({
    behaviorType: BehaviorType.AddToCart,
    productId,
    sourcePage: sourcePage || window?.location?.pathname,
  });
}

/** Track a purchase (call for each product in the order) */
export function trackPurchase(productId: number): void {
  trackBehavior({
    behaviorType: BehaviorType.Purchase,
    productId,
    sourcePage: 'checkout',
  });
}

/** Track adding to wishlist */
export function trackWishlist(productId: number): void {
  trackBehavior({
    behaviorType: BehaviorType.Wishlist,
    productId,
    sourcePage: window?.location?.pathname,
  });
}

/** Track a product rating */
export function trackRating(productId: number, score: number): void {
  trackBehavior({
    behaviorType: BehaviorType.Rating,
    productId,
    ratingScore: score,
    sourcePage: window?.location?.pathname,
  });
}

/** Track adding to comparison */
export function trackCompare(productId: number): void {
  trackBehavior({
    behaviorType: BehaviorType.Compare,
    productId,
    sourcePage: window?.location?.pathname,
  });
}

/**
 * Track dwell time on a product page.
 * Call this when leaving the product page with the elapsed seconds.
 */
export function trackDwellTime(productId: number, seconds: number): void {
  trackBehavior({
    behaviorType: BehaviorType.View,
    productId,
    dwellTimeSeconds: Math.round(seconds),
    sourcePage: window?.location?.pathname,
  });
}

// ══════════════════════════════════════════
//  Recommendation fetching
// ══════════════════════════════════════════

export async function getPersonalizedRecommendations(
  limit = 8
): Promise<RecommendedProduct[]> {
  try {
    const res = await axios.get(`${API_URL}/behavior/recommendations`, {
      params: { sessionId: getSessionId(), limit },
      headers: getAuthHeaders(),
    });
    return res.data?.data || [];
  } catch {
    return [];
  }
}

export async function getRecentlyViewed(limit = 8): Promise<RecommendedProduct[]> {
  try {
    const res = await axios.get(`${API_URL}/behavior/recently-viewed`, {
      params: { sessionId: getSessionId(), limit },
      headers: getAuthHeaders(),
    });
    return res.data?.data || [];
  } catch {
    return [];
  }
}

export async function getBoughtTogether(
  productId: number,
  limit = 5
): Promise<RecommendedProduct[]> {
  try {
    const res = await axios.get(`${API_URL}/behavior/bought-together/${productId}`, {
      params: { limit },
      headers: getAuthHeaders(),
    });
    return res.data?.data || [];
  } catch {
    return [];
  }
}

export async function getAlsoViewed(
  productId: number,
  limit = 5
): Promise<RecommendedProduct[]> {
  try {
    const res = await axios.get(`${API_URL}/behavior/also-viewed/${productId}`, {
      params: { limit },
      headers: getAuthHeaders(),
    });
    return res.data?.data || [];
  } catch {
    return [];
  }
}
