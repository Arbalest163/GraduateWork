import { FC, ReactElement } from "react";
import down_arrow from '../images/down-arrow.png';
import './images-style.css';

const DownArrowButton : FC<{}> = () : ReactElement => {
    return(
        <img src={down_arrow} className="down-arrow-img pointer-hover"/>
    );
}

export default DownArrowButton;