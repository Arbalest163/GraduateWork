import { ChangeEvent, FC, ReactElement, useRef } from "react";

interface AvatarProps {
    source: string;
    onAvatarChange: (base64: string) => void;
}

const ChangeAvatarComponent : FC<AvatarProps> = ({source, onAvatarChange}) : ReactElement => {
    const fileInputRef = useRef<HTMLInputElement>(null);

    const handleAvatarChange = (event: ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (file) {
            const reader = new FileReader();
            reader.onloadend = () => {
                const base64 = reader.result as string;
                onAvatarChange(base64);
            };
            reader.readAsDataURL(file);
        }
    };

    const handleAvatarClick = () => {
        if (fileInputRef.current) {
        fileInputRef.current.click();
        }
    };
    return(
        <div className="avatar-container-edit">
            <img
                src={source}
                alt="Avatar"
                className="pointer-hover"
                onClick={handleAvatarClick}
            />
            <input
                type="file"
                accept=".jpg, .jpeg, .png"
                onChange={handleAvatarChange}
                ref={fileInputRef}
                style={{ display: "none" }}
            />
        </div>
        
    );
}

export default ChangeAvatarComponent;