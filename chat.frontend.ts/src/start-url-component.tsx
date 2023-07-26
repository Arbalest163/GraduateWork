import { FC, ReactElement, useEffect } from "react";
import { useNavigate } from "react-router-dom";

const StartUrlComponent : FC<{}> = (): ReactElement => {
  const navigate = useNavigate();

  useEffect(() => {
    navigate('/chats');
  }, []);

  return (<></>);
}

export default StartUrlComponent;