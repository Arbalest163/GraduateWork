import { Menu } from "@headlessui/react";
import { FC, Fragment, ReactElement } from "react";
import './action-chats-component.css'
import { Button } from "../api/models/models";

interface ActionChatsProps {
    buttons: Button[];
    imageMenuButton: () => ReactElement;
}

const ActionChatsComponent: FC<ActionChatsProps> = ({buttons, imageMenuButton}) : ReactElement => {
    
    return (
        <div>
            <Menu as="div" className="menu">
                <Menu.Button className="small-active-button">{imageMenuButton}</Menu.Button>
                <Menu.Items className="menu-items bg-white">
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
            </Menu>
        </div>
    )
}

export default ActionChatsComponent;