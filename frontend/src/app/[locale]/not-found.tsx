import { Button } from "@/components/ui/button";
import Link from "next/link";
import { useLocale } from "next-intl";
import { Home } from "lucide-react";

export default function NotFound() {
  const locale = useLocale();

  return (
    <div className="flex min-h-[60vh] items-center justify-center px-4">
      <div className="text-center space-y-6 max-w-md">
        <div className="text-6xl font-bold text-muted-foreground">404</div>
        <h2 className="text-2xl font-bold">Page Not Found</h2>
        <p className="text-muted-foreground">
          The page you are looking for does not exist or has been moved.
        </p>
        <Link href={`/${locale}`}>
          <Button className="gap-2">
            <Home className="h-4 w-4" />
            Go Home
          </Button>
        </Link>
      </div>
    </div>
  );
}
