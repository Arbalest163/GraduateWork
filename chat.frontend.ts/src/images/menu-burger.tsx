import { FC, ReactElement } from "react";
import burger_menu from '../images/icon-font-menu-burger.png';

const MenuBurger : FC<{}> = () : ReactElement => {
    return(
        <img src={burger_menu} className="image-input pointer-hover"/>
    );
}

export default MenuBurger;