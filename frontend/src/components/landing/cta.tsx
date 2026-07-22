"use client";

import { useTranslations, useLocale } from "next-intl";
import { Button } from "@/components/ui/button";
import Link from "next/link";
import { ArrowRight } from "lucide-react";

export function CTASection() {
  const t = useTranslations("cta");
  const locale = useLocale();

  return (
    <section className="py-20 md:py-32 bg-primary text-primary-foreground relative overflow-hidden">
      <div className="absolute inset-0 bg-gradient-to-br from-primary via-primary/90 to-primary" />
      <div className="container mx-auto px-4 text-center space-y-8 relative">
        <h2 className="text-3xl md:text-4xl lg:text-5xl font-bold">{t("title")}</h2>
        <p className="text-primary-foreground/80 max-w-xl mx-auto text-lg">
          {t("subtitle")}
        </p>
        <div>
          <Link href={`/${locale}/register`}>
            <Button
              size="lg"
              variant="secondary"
              className="gap-2 px-8 text-base"
            >
              {t("button")}
              <ArrowRight className="h-4 w-4" />
            </Button>
          </Link>
        </div>
        <p className="text-sm text-primary-foreground/60">{t("note")}</p>
      </div>
    </section>
  );
}
