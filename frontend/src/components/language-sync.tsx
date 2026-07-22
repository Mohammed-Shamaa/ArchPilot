"use client";

import { useEffect } from "react";
import { useLanguageStore } from "@/stores/language-store";

export function LanguageSync({ children }: { children: React.ReactNode }) {
  const { locale } = useLanguageStore();

  useEffect(() => {
    document.cookie = `NEXT_LOCALE=${locale};path=/;max-age=31536000`;
  }, [locale]);

  return <>{children}</>;
}
