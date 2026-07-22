"use client";

import { useTranslations, useLocale } from "next-intl";

export function Footer() {
  const t = useTranslations("footer");
  const locale = useLocale();

  return (
    <footer className="border-t bg-muted/50">
      <div className="container mx-auto px-4 py-12">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
          <div className="space-y-4">
            <div className="flex items-center space-x-2">
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
              <span className="font-bold">ArchPilot</span>
            </div>
            <p className="text-sm text-muted-foreground max-w-xs">
              {t("description")}
            </p>
          </div>

          <div>
            <h4 className="font-semibold mb-3">{t("product")}</h4>
            <ul className="space-y-2 text-sm text-muted-foreground">
              <li>{t("features")}</li>
              <li>{t("pricing")}</li>
              <li>{t("docs")}</li>
            </ul>
          </div>

          <div>
            <h4 className="font-semibold mb-3">{t("company")}</h4>
            <ul className="space-y-2 text-sm text-muted-foreground">
              <li>{t("about")}</li>
              <li>{t("blog")}</li>
              <li>{t("careers")}</li>
              <li>{t("contact")}</li>
            </ul>
          </div>

          <div>
            <h4 className="font-semibold mb-3">{t("legal")}</h4>
            <ul className="space-y-2 text-sm text-muted-foreground">
              <li>{t("privacy")}</li>
              <li>{t("terms")}</li>
            </ul>
          </div>
        </div>

        <div className="mt-8 pt-8 border-t text-center text-sm text-muted-foreground">
          {t("copyright")}
        </div>
      </div>
    </footer>
  );
}
