import { FluentDynamicStylesheet } from "@/utils";
import { KeyboardEvent, useCallback, useRef } from "react";
import { useUnit } from "effector-react/scope";
import { ContextualMenu, IconButton, IContextualMenuItem, SearchBox, Spinner, Label, Stack } from "@fluentui/react";
import * as model from "./model.ts";

interface IProps {
    label?: string;
    placeholder?: string;
    value?: string;
    freeInputAllowed?: boolean;
    showSearchIcon?: boolean;
}

const { ReceivedDataGate } = model;

export const TextSuggestions = ({
    label = "Select:",
    placeholder = "Start typing to see available options...",
    value,
    freeInputAllowed = false,
    showSearchIcon = false,
}: IProps) => {
    const [query, debouncedQuery, normalizedOptions, pending, isMenuVisible, contextMenuFocused] = useUnit([
        model.$query,
        model.$debouncedQuery,
        model.$normalizedOptions,
        model.$pending,
        model.$isMenuVisible,
        model.$contextMenuFocused,
    ]);

    const [changeQuery, changeFocused, selectOption, changeContextMenuFocused] = useUnit([
        model.changeQuery,
        model.changeFocused,
        model.selectOption,
        model.changeContextMenuFocused,
    ]);

    const searchBoxRef = useRef<HTMLDivElement>(null);
    const contextMenuRef = useRef<HTMLDivElement>(null);

    const handleOptionSelected = useCallback(
        (_: unknown, item?: IContextualMenuItem) => {
            const value = item?.text ?? "";
            selectOption(value);
        },
        [selectOption],
    );

    const handleInputValueChanged = useCallback(
        (_?: unknown, text?: string) => {
            changeContextMenuFocused(false);
            changeQuery(text ?? "");
        },
        [changeContextMenuFocused, changeQuery],
    );

    const handleFocused = useCallback(() => {
        if (!isMenuVisible) {
            changeFocused(true);
        }
    }, [changeFocused, isMenuVisible]);

    const handleBlur = () => {
        const optionsEmpty = normalizedOptions.length === 0;
        const queryAndValueEqual = query === value;

        const shouldResetOption = !isMenuVisible && optionsEmpty && !queryAndValueEqual && !freeInputAllowed;

        if (shouldResetOption) {
            selectOption(value ?? "");
        }
    };

    const handleListDismissed = useCallback(() => {
        changeFocused(false);
    }, [changeFocused]);

    const handleTextCleared = useCallback(() => {
        selectOption("");
    }, [selectOption]);

    const handleKeyUp = (e: KeyboardEvent<HTMLInputElement>) => {
        switch (e.key) {
            case "ArrowUp":
            case "Tab":
            case "ArrowDown":
                {
                    if (debouncedQuery.length > 0) e.preventDefault();
                    changeContextMenuFocused(true);
                }
                break;

            case "Enter": {
                const firstOption = normalizedOptions.at(0);

                if (firstOption) {
                    e.preventDefault();
                    selectOption(firstOption.text ?? "");
                    changeFocused(false);
                    searchBoxRef.current?.blur();
                }
            }
        }
    };

    const rightIcon = !pending && Boolean(debouncedQuery) ? "Clear" : undefined;

    return (
        <FluentDynamicStylesheet>
            <ReceivedDataGate
                freeInputAllowed={freeInputAllowed}
                rawValue={value ?? ""}
                triggerNode={searchBoxRef.current}
            />

            <>
                <Label>{label}</Label>

                <Stack
                    horizontal
                    verticalAlign={"center"}
                    tokens={{ childrenGap: 5 }}
                >
                    <Stack.Item grow={1}>
                        <SearchBox
                            ref={searchBoxRef}
                            onChange={handleInputValueChanged}
                            placeholder={placeholder}
                            onFocus={handleFocused}
                            onBlur={handleBlur}
                            value={query}
                            autoFocus={false}
                            onKeyUp={handleKeyUp}
                            clearButtonProps={{ hidden: true }}
                            showIcon={showSearchIcon}
                            underlined
                        />
                    </Stack.Item>

                    {query && (
                        <Stack.Item>
                            <IconButton
                                iconProps={{ iconName: rightIcon }}
                                onClick={pending ? undefined : handleTextCleared}
                            >
                                {pending && <Spinner />}
                            </IconButton>
                        </Stack.Item>
                    )}
                </Stack>

                {!pending && (
                    <ContextualMenu
                        items={normalizedOptions}
                        target={searchBoxRef.current}
                        hidden={!isMenuVisible}
                        useTargetAsMinWidth
                        useTargetWidth
                        onItemClick={handleOptionSelected}
                        shouldFocusOnMount={contextMenuFocused}
                        ref={contextMenuRef}
                        onDismiss={handleListDismissed}
                    />
                )}
            </>
        </FluentDynamicStylesheet>
    );
};

export const TextSuggestionDefaultContract = {
    props: {
        label: "string",
        value: "string",
        placeholder: "string",
        freeInputAllowed: "boolean",
        showSearchIcon: "boolean",
    },
    shadow: "open" as const,
};
