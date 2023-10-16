import { registerWebComponent } from "@/utils";

import { CounterComponent } from "./counter";

import { FluentInputMaskComponent } from "./fluent/fluent-input-mask";
import {
    AddressSuggestionComponent,
    InitialsSuggestionComponent,
    CompanySuggestionComponent,
} from "./fluent/text-suggestions/dadata-suggestions";

export const initWebComponents = () => {
    registerWebComponent("counter-component", CounterComponent);
    registerWebComponent("fluent-input-mask-component", FluentInputMaskComponent);

    registerWebComponent("address-suggestion-component", AddressSuggestionComponent);
    registerWebComponent("initials-suggestion-component", InitialsSuggestionComponent);
    registerWebComponent("company-suggestion-component", CompanySuggestionComponent);
};
