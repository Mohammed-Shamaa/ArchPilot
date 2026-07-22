import { create } from "zustand";
import { persist } from "zustand/middleware";
import { type Locale } from "@/i18n/config";

interface LanguageState {
  locale: Locale;
  setLocale: (locale: Locale) => void;
}

export const useLanguageStore = create<LanguageState>()(
  persist(
    (set) => ({
      locale: "en",
      setLocale: (locale) => {
        document.cookie = `NEXT_LOCALE=${locale};path=/;max-age=31536000`;
        set({ locale });
      },
    }),
    { name: "archpilot-language" }
  )
);
