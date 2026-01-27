"use client";

import * as React from "react";

type Theme = "light" | "dark" | "system";

interface ThemeContextType {
    theme: Theme;
    setTheme: (theme: Theme) => void;
    resolvedTheme: "light" | "dark";
}

const ThemeContext = React.createContext<ThemeContextType | undefined>(undefined);

export function useTheme() {
    const context = React.useContext(ThemeContext);
    if (!context) {
        throw new Error("useTheme must be used within a ThemeProvider");
    }
    return context;
}

interface ThemeProviderProps {
    children: React.ReactNode;
    defaultTheme?: Theme;
    storageKey?: string;
}

export function ThemeProvider({
    children,
    defaultTheme = "system",
    storageKey = "shop-tts-theme",
}: ThemeProviderProps) {
    const [theme, setThemeState] = React.useState<Theme>(defaultTheme);
    const [resolvedTheme, setResolvedTheme] = React.useState<"light" | "dark">("light");

    // Initialize theme from localStorage
    React.useEffect(() => {
        const stored = localStorage.getItem(storageKey) as Theme | null;
        if (stored) {
            setThemeState(stored);
        }
    }, [storageKey]);

    // Update resolved theme and apply to document
    React.useEffect(() => {
        const root = window.document.documentElement;

        const getResolvedTheme = (): "light" | "dark" => {
            if (theme === "system") {
                return window.matchMedia("(prefers-color-scheme: dark)").matches
                    ? "dark"
                    : "light";
            }
            return theme;
        };

        const resolved = getResolvedTheme();
        setResolvedTheme(resolved);

        root.classList.remove("light", "dark");
        root.classList.add(resolved);
    }, [theme]);

    // Listen for system theme changes
    React.useEffect(() => {
        if (theme !== "system") return;

        const mediaQuery = window.matchMedia("(prefers-color-scheme: dark)");

        const handleChange = (e: MediaQueryListEvent) => {
            setResolvedTheme(e.matches ? "dark" : "light");
            document.documentElement.classList.remove("light", "dark");
            document.documentElement.classList.add(e.matches ? "dark" : "light");
        };

        mediaQuery.addEventListener("change", handleChange);
        return () => mediaQuery.removeEventListener("change", handleChange);
    }, [theme]);

    const setTheme = React.useCallback((newTheme: Theme) => {
        localStorage.setItem(storageKey, newTheme);
        setThemeState(newTheme);
    }, [storageKey]);

    return (
        <ThemeContext.Provider value={{ theme, setTheme, resolvedTheme }}>
            {children}
        </ThemeContext.Provider>
    );
}
