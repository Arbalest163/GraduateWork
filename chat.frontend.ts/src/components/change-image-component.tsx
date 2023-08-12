import { ChangeEvent, FC, ReactElement, useRef } from "react";

interface ChangeImageProps {
    source: string;
    onImageChange: (base64: string) => void;
    description?: string;
}

const ChangeImageComponent : FC<ChangeImageProps> = ({source, onImageChange: onImageChange, description}) : ReactElement => {
    const fileInputRef = useRef<HTMLInputElement>(null);

    const handleAvatarChange = (event: ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (file) {
            const reader = new FileReader();
            reader.onloadend = () => {
                const base64 = reader.result as string;
                onImageChange(base64);
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
        <div className="image-container-edit">
            <img
                src={source}
                alt={description ? description : 'Image'}
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

export default ChangeImageComponent;