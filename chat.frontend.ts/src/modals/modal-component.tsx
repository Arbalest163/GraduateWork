import { Dialog } from "@headlessui/react";
import { FC, ReactElement } from "react";
import useModalContext from "../hooks/useModalContext";
import './modal-component.css';

const ModalComponent : FC<{}> = (): ReactElement => {
    const {visible, content, closeModal} = useModalContext();

    return (
        <div>
            <Dialog open={visible} onClose={closeModal}>
                {content}
            </Dialog>
        </div>
    )
}

export default ModalComponent;