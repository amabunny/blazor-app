import { Record, String, Array, Static } from "runtypes";

export const SuggestionContract = Record({
    value: String,
    unrestricted_value: String,
});
export type Suggestion = Static<typeof SuggestionContract>;

export const SuggestionsListContract = Array(SuggestionContract);
export type SuggestionsList = Static<typeof SuggestionsListContract>;
