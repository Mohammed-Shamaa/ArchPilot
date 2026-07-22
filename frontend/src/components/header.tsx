"use client";

import Link from "next/link";
import { useTranslations, useLocale } from "next-intl";
import { Button } from "@/components/ui/button";
import { useAuthStore } from "@/stores/auth-store";
import { useLanguageStore } from "@/stores/language-store";
import { useThemeStore } from "@/stores/theme-store";
import { Moon, Sun, Globe } from "lucide-react";

export function Header() {
  const t = useTranslations("nav");
  const locale = useLocale();
  const { isAuthenticated, logout } = useAuthStore();
  const { setLocale } = useLanguageStore();
  const { theme, setTheme } = useThemeStore();
  const isRTL = locale === "ar";

  const toggleLanguage = () => {
    const newLocale = locale === "en" ? "ar" : "en";
    setLocale(newLocale);
    document.cookie = `NEXT_LOCALE=${newLocale};path=/;max-age=31536000`;
    window.location.reload();
  };

  const toggleTheme = () => {
    setTheme(theme === "dark" ? "light" : "dark");
  };

  return (
    <header className="sticky top-0 z-50 w-full border-b bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60">
      <div className="container mx-auto flex h-14 items-center justify-between px-4">
        <Link href={`/${locale}`} className="flex items-center space-x-2">
          <svg
            className="h-6 w-6 text-primary"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            strokeWidth={2}
          >
            <path d="M12 2L2 7l10 5 10-5-10-5z" />
            <path d="M2 17l10 5 10-5" />
            <path d="M2 12l10 5 10-5" />
          </svg>
          <span className="font-bold">{t("home") === "Home" ? "ArchPilot" : "آرك بايلوت"}</span>
        </Link>

        <nav className="hidden md:flex items-center gap-6">
          <Link
            href={`/${locale}`}
            className="text-sm font-medium text-muted-foreground hover:text-foreground transition-colors"
          >
            {t("home")}
          </Link>
          <Link
            href={`/${locale}#features`}
            className="text-sm font-medium text-muted-foreground hover:text-foreground transition-colors"
          >
            {t("features")}
          </Link>
          {isAuthenticated && (
            <Link
              href={`/${locale}/dashboard`}
              className="text-sm font-medium text-muted-foreground hover:text-foreground transition-colors"
            >
              {t("dashboard")}
            </Link>
          )}
        </nav>

        <div className="flex items-center gap-2">
          <Button
            variant="ghost"
            size="icon"
            onClick={toggleLanguage}
            title={isRTL ? "Switch to English" : "التبديل إلى العربية"}
          >
            <Globe className="h-4 w-4" />
          </Button>
          <Button variant="ghost" size="icon" onClick={toggleTheme}>
            {theme === "dark" ? (
              <Sun className="h-4 w-4" />
            ) : (
              <Moon className="h-4 w-4" />
            )}
          </Button>

          {isAuthenticated ? (
            <Button variant="ghost" size="sm" onClick={logout}>
              {t("logout")}
            </Button>
          ) : (
            <>
              <Link href={`/${locale}/login`}>
                <Button variant="ghost" size="sm">
                  {t("login")}
                </Button>
              </Link>
              <Link href={`/${locale}/register`}>
                <Button size="sm">{t("register")}</Button>
              </Link>
            </>
          )}
        </div>
      </div>
    </header>
  );
}
