import r2wc from "@r2wc/react-to-web-component";
import { suggestAddressAsync, suggestCompanyAsync, suggestInitialsAsync } from "@/services/dadata";
import { withForkedDomain } from "@/utils/with-forked-domain.tsx";
import { TextSuggestionDefaultContract, TextSuggestions } from "./index.tsx";
import { domain, getSuggestionsFx } from "./model";

export const AddressSuggestionComponent = r2wc(
    withForkedDomain({
        domain,
        Component: TextSuggestions,
        handlers: new Map().set(getSuggestionsFx, suggestAddressAsync),
    }),
    TextSuggestionDefaultContract,
);

export const InitialsSuggestionComponent = r2wc(
    withForkedDomain({
        domain,
        Component: TextSuggestions,
        handlers: new Map().set(getSuggestionsFx, suggestInitialsAsync),
    }),
    TextSuggestionDefaultContract,
);

export const CompanySuggestionComponent = r2wc(
    withForkedDomain({
        domain,
        Component: TextSuggestions,
        handlers: new Map().set(getSuggestionsFx, suggestCompanyAsync),
    }),
    TextSuggestionDefaultContract,
);
