import { useCallback, useEffect, useId, useRef } from "react";
import r2wc from "@r2wc/react-to-web-component";
import { DefaultButton, PrimaryButton } from "@fluentui/react/lib/Button";
import { TooltipHost } from "@fluentui/react/lib/Tooltip";
import { Stack } from "@fluentui/react/lib/Stack";
import { FluentDynamicStylesheet, registerCustomEvents, createCustomEvent } from "@/utils";

type CounterWebComponentChangeArgs = {
    increment?: boolean;
    decrement?: boolean;
};

const changeEvent = createCustomEvent<CounterWebComponentChangeArgs, "countervaluechanged">("countervaluechanged");

interface IProps {
    value?: number;
}

const Counter = ({ value }: IProps) => {
    const id = useId();
    const rootRef = useRef<HTMLDivElement>(null);

    const handleIncrement = useCallback(() => {
        changeEvent.trigger(rootRef.current, {
            increment: true,
        });
    }, []);

    const handleDecrement = () => {
        changeEvent.trigger(rootRef.current, {
            decrement: true,
        });
    };

    const handlePerformHugeIncrement = () => {
        Array.from({ length: 10 }).forEach(() => {
            changeEvent.trigger(rootRef.current, {
                increment: true,
            });
        });
    };

    useEffect(() => {
        registerCustomEvents((register) => {
            register.registerEvent(changeEvent);
        });
    }, []);

    return (
        <>
            <FluentDynamicStylesheet />

            <div ref={rootRef}>
                <Stack
                    horizontal
                    tokens={{ childrenGap: 10 }}
                    verticalAlign={"start"}
                >
                    <slot name="icon" />

                    <h4>
                        <b>React</b> Counter <b>web-component</b> value: {value}
                    </h4>
                </Stack>

                <Stack
                    horizontal
                    tokens={{ childrenGap: 20 }}
                    verticalAlign={"center"}
                >
                    <slot name="children"></slot>

                    <TooltipHost
                        content={"React handles some things here :)"}
                        id={id}
                    >
                        <DefaultButton onClick={handleIncrement}>Increment</DefaultButton>
                    </TooltipHost>

                    <DefaultButton onClick={handleDecrement}>Decrement</DefaultButton>

                    <PrimaryButton onClick={handlePerformHugeIncrement}>
                        Perform huge increment from React
                    </PrimaryButton>
                </Stack>
            </div>
        </>
    );
};

export const CounterComponent = r2wc(Counter, {
    props: {
        value: "number",
    },
    shadow: "open",
});
