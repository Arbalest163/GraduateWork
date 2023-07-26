import { Dialog } from "@headlessui/react";
import { FC, ReactElement, useState } from "react";
import useModalContext from "../hooks/useModalContext";

interface ErrorModalProps {
    text: string;
}

const ErrorModalComponent : FC<ErrorModalProps> = ({text}) : ReactElement => {
    const {closeModal} = useModalContext();

    return (
        <div className="bg">
             <Dialog.Panel className="popup-error">
                <div className="modal-title">Ошибка!</div>
                <div>{text}</div>
                <button className="button-popup" onClick={closeModal}>ОК</button>
            </Dialog.Panel>
        </div>
    );
}

export const ErrorModal = (text: string) : ReactElement => {
    return (
        <ErrorModalComponent text={text}></ErrorModalComponent>
    );
}

export default ErrorModalComponent;