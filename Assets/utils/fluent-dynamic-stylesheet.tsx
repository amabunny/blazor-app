import { initializeIcons } from "@fluentui/react/lib/Icons";
import { useLayoutEffect, useRef, memo, useMemo, PropsWithChildren, useState } from "react";

initializeIcons();

export const FluentDynamicStylesheet = memo(({ children }: PropsWithChildren) => {
    const [stylesheetLoaded, setStylesheetLoaded] = useState(false);
    const stylesheet = useRef<HTMLStyleElement>(null);

    useLayoutEffect(() => {
        if (!stylesheet) {
            console.warn("Something went wrong...");
            return () => {};
        }

        const adoptGeneratedStyles = (nodeList: NodeList, cb?: () => void) => {
            if (nodeList.length === 0) return;

            nodeList.forEach((style) => {
                if (style instanceof HTMLStyleElement) {
                    Array.from(style.sheet?.cssRules ?? []).forEach((rule, index) => {
                        stylesheet.current?.sheet?.insertRule(rule.cssText, index);
                    });
                }
            });

            cb?.();
        };

        adoptGeneratedStyles(document.querySelectorAll("style[data-merge-styles]"), () => {
            setStylesheetLoaded(true);
        });

        const observer = new MutationObserver((mutationList) => {
            mutationList.forEach((mutation) => {
                adoptGeneratedStyles(mutation.addedNodes);
            });
        });

        observer.observe(document.head, {
            childList: true,
            subtree: true,
        });

        return () => {
            observer.disconnect();
        };
    }, []);

    const links = useMemo(() => {
        return Array.from(document.querySelectorAll('link[href*="Fluent"]')).map((link, index) => ({
            rel: link.getAttribute("rel") ?? undefined,
            href: link.getAttribute("href") ?? undefined,
            key: link.getAttribute("href") ?? index.toString(),
        }));
    }, []);

    return (
        <>
            <style ref={stylesheet} />

            {links.map((link) => (
                <link
                    rel={link.rel}
                    href={link.href}
                    key={link.key}
                />
            ))}

            {stylesheetLoaded && children}
        </>
    );
});
