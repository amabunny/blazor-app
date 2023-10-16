import { SuggestionsList } from "@/services/dadata";
import { createCustomEvent, registerCustomEvents } from "@/utils";
import { IContextualMenuItem } from "@fluentui/react";
import { attach, combine, createDomain, forward, guard, restore } from "effector";
import { createGate } from "effector-react/scope";
import { debounce } from "patronum";

const eventName = "textsuggestionchanged";
interface TextSuggestionsChangeArgs {
    value: string;
}
const textSuggestionsChanged = createCustomEvent<TextSuggestionsChangeArgs, typeof eventName>("textsuggestionchanged");

const domain = createDomain("TextSuggestionsDomain");
const { createEffect, createStore, createEvent } = domain;

interface IReceivedDataGate {
    rawValue: string;
    freeInputAllowed: boolean;
    triggerNode: Element | null;
}
const ReceivedDataGate = createGate<IReceivedDataGate>({ domain });

const changeQuery = createEvent<string>();
const changeFocused = createEvent<boolean>();
const changeContextMenuFocused = createEvent<boolean>();
const clearSuggestions = createEvent();
const selectOption = createEvent<string>();

const getSuggestionsFx = createEffect({
    handler: (query: string): SuggestionsList => {
        void query;
        return [];
    },
});

const registerEventFx = createEffect({
    handler: () => {
        registerCustomEvents((register) => {
            register.registerEvent(textSuggestionsChanged);
        });
    },
});

interface TriggerChangeEventOnNodeFxParams {
    node: Element | null;
    value: string;
}
const triggerChangeEventOnNodeFx = attach({
    effect: createEffect({
        handler: ({ node, value }: TriggerChangeEventOnNodeFxParams) => {
            textSuggestionsChanged.trigger(node, { value });
        },
    }),
    source: ReceivedDataGate.state.map((data): Element | null => data.triggerNode),
    mapParams: (value: string, node: Element | null) => ({
        node,
        value,
    }),
});

const $query = restore(changeQuery, "").on(
    ReceivedDataGate.state.map(({ rawValue }) => rawValue),
    (_, query) => query,
);
const $debouncedQuery = restore(debounce({ source: $query, timeout: 1000 }), "");
const $focused = restore(changeFocused, false);
const $selectedOption = restore(selectOption, null);
const $contextMenuFocused = restore(changeContextMenuFocused, false);

const $pending = createStore(false)
    .on(combine($query, $focused), (_, [query, focused]) => query.length > 0 && focused)
    .on(getSuggestionsFx.finally, () => false);

const $options = createStore<SuggestionsList>([])
    .on(getSuggestionsFx.doneData, (_, suggestions) => suggestions)
    .on(clearSuggestions, () => []);

const $normalizedOptions = combine($options, $debouncedQuery, (options, query) =>
    options.map(
        (option, index): IContextualMenuItem => ({
            text: option.value,
            key: index.toString(),
            checked: option.value === query || index === 0,
        }),
    ),
);

const $isUnknownQuery = combine(ReceivedDataGate.state, $query, ({ rawValue }, query) => rawValue !== query);

const $isMenuVisible = combine(
    $focused,
    $debouncedQuery,
    $normalizedOptions,
    (focused, debouncedQuery, normalizedAddresses) =>
        focused && debouncedQuery.length > 0 && normalizedAddresses.length > 0,
);

guard({
    clock: combine($debouncedQuery, $focused),
    filter: ([debouncedQuery, focused]) => debouncedQuery.length > 0 && focused,
    target: attach({
        effect: getSuggestionsFx,
        source: $query,
    }),
});

guard({
    clock: changeQuery,
    filter: $query.map((query) => query.length === 0),
    target: clearSuggestions,
});

guard({
    clock: combine($isUnknownQuery, $focused, ReceivedDataGate.state, $query),
    filter: ([isUnknownQuery, focused, { freeInputAllowed }]) =>
        focused === false && isUnknownQuery && freeInputAllowed,
    target: triggerChangeEventOnNodeFx.prepend(([, , , query]: [unknown, unknown, unknown, string]) => query),
});

guard({
    clock: combine($isUnknownQuery, $focused, ReceivedDataGate.state, $selectedOption, $query),
    filter: ([isUnknownQuery, focused, { freeInputAllowed }, selectedOption, query]) =>
        [focused === false, isUnknownQuery, !freeInputAllowed, selectedOption !== query].every(Boolean),
    target: selectOption.prepend(() => ""),
});

forward({
    from: ReceivedDataGate.open,
    to: registerEventFx,
});

forward({
    from: selectOption,
    to: [changeQuery, triggerChangeEventOnNodeFx],
});

export {
    $pending,
    $query,
    $debouncedQuery,
    $focused,
    $isMenuVisible,
    $normalizedOptions,
    $contextMenuFocused,
    $selectedOption,
    selectOption,
    getSuggestionsFx,
    triggerChangeEventOnNodeFx,
    changeQuery,
    changeFocused,
    changeContextMenuFocused,
    clearSuggestions,
    domain,
    ReceivedDataGate,
};
