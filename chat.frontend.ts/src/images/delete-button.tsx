import { FC, ReactElement } from "react";
import delete_button from '../images/delete-button.png';
import './images-style.css';


const DeleteButton : FC<{}> = () : ReactElement => {
    return(
        <img src={delete_button} className="delete-button-img pointer-hover"/>
    );
}

export default DeleteButton;