import { Menu, Transition } from "@headlessui/react";
import { FC, Fragment, ReactElement, useEffect, useRef, useState } from "react";
import './action-chats-component.css'
import { Button, OpeningDirection } from "../api/models/models";

interface ActionChatsProps {
    buttons: Button[];
    imageMenuButton: () => ReactElement;
}

const ActionChatsComponent: FC<ActionChatsProps> = ({buttons, imageMenuButton}) : ReactElement => {
    const [openingDirection, setOpeningDirection] = useState<OpeningDirection>(OpeningDirection.DownLeft);
    const menuItemsRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        if (menuItemsRef.current) {
            const menuItemsRect = menuItemsRef.current.getBoundingClientRect();
            const { top, bottom, left, right } = menuItemsRect;

            const windowWidth = window.innerWidth;
            const windowHeight = window.innerHeight;

            const topPosition = top;
            const bottomPosition = windowHeight - bottom;
            const leftPosition = left;
            const rightPosition = windowWidth - right;

            if(bottomPosition > topPosition) {
                if(leftPosition > rightPosition) {
                    setOpeningDirection(OpeningDirection.DownLeft);
                } else {
                    setOpeningDirection(OpeningDirection.DownRight);
                }
                
            } else {
                if(leftPosition > rightPosition) {
                    setOpeningDirection(OpeningDirection.UpLeft);
                } else {
                    setOpeningDirection(OpeningDirection.UpRight);
                }
            }
        }
    },[]);

    return (
        <div>
            <Menu as="div" className="menu" ref={menuItemsRef}>
                <Menu.Button className="small-active-button">{imageMenuButton}</Menu.Button>
                <Transition
                    as={Fragment}
                    enter="transition ease-out duration-100"
                    enterFrom="transform opacity-0 scale-95"
                    enterTo="transform opacity-100 scale-100"
                    leave="transition ease-in duration-75"
                    leaveFrom="transform opacity-100 scale-100"
                    leaveTo="transform opacity-0 scale-95"
                >
                    <Menu.Items  className={`menu-items bg-white ${openingDirection.toString()}`}>
                        {buttons.map((button) => (
                            <Menu.Item key={button.label} as={Fragment}>
                                {({ active }) => (
                                <button onClick={button.onClick} className={`${active ? 'bg-chats-menu-item text-white' : 'text-black'} menu-item bg-white`}>
                                    {button.label}
                                </button>
                                )}
                            </Menu.Item>
                        ))}
                    </Menu.Items>
                </Transition>
            </Menu>
        </div>
    )
}

export default ActionChatsComponent;