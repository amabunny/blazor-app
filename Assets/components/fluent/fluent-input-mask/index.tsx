import { MaskedTextField } from "@fluentui/react/lib/TextField";
import { Label } from "@fluentui/react/lib/Label";
import r2wc from "@r2wc/react-to-web-component";
import { useEffect, useRef, FormEvent } from "react";
import { FluentDynamicStylesheet, createCustomEvent, registerCustomEvents } from "@/utils";

interface IProps {
    mask: string;
    label: string;
    prefix: string;
    value?: string;
}

const changeInputMaskValue = createCustomEvent<{ value: string }, "maskvaluechanged">("maskvaluechanged");

const FluentInputMask = ({ mask, label, value, prefix }: IProps) => {
    const rootRef = useRef<HTMLDivElement>(null);

    const handleChange = (_: FormEvent, newValue?: string) => {
        changeInputMaskValue.trigger(rootRef.current, {
            value: newValue ?? "",
        });
    };

    useEffect(() => {
        registerCustomEvents((register) => {
            register.registerEvent(changeInputMaskValue);
        });
    }, []);

    return (
        <>
            <FluentDynamicStylesheet />

            <div ref={rootRef}>
                <Label>{label}</Label>

                <MaskedTextField
                    prefix={prefix}
                    value={value}
                    mask={mask}
                    onChange={handleChange}
                    underlined
                />
            </div>
        </>
    );
};

export const FluentInputMaskComponent = r2wc(FluentInputMask, {
    props: {
        mask: "string",
        label: "string",
        defaultValue: "string",
        value: "string",
        prefix: "string",
    },
    shadow: "open",
});
