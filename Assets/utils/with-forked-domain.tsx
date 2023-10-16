import { SuggestionsList } from "@/services/dadata";
import { Domain, Effect, fork, ValueMap } from "effector";
import { attachLogger } from "effector-logger";
import { Provider } from "effector-react/scope";
import { ComponentType, useEffect, useMemo } from "react";

interface IDebugParams {
    name?: string;
    enabled?: true;
}

interface WithForkedDomainArgs {
    domain: Domain;
    Component: ComponentType;
    values?: ValueMap;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    handlers?: Map<Effect<any, any, any>, (query: string) => SuggestionsList | Promise<SuggestionsList>>;
    debugParams?: IDebugParams;
}

export const withForkedDomain = ({ domain, debugParams, Component, values, handlers }: WithForkedDomainArgs) => {
    function WithForkedDomain(props: Record<string, unknown>) {
        const scope = useMemo(() => fork(domain, { values, handlers }), []);

        useEffect(() => {
            if (debugParams) {
                const unsub = attachLogger({ scope, name: debugParams.name });

                return () => {
                    unsub();
                };
            }

            return () => {};
        }, [scope]);

        return (
            <Provider value={scope}>
                <Component {...props} />
            </Provider>
        );
    }

    return WithForkedDomain;
};
