export const stringEquals = (str1: string, str2: string) => {
    const lowercaseStr1 = str1.toLowerCase();
    const lowercaseStr2 = str2.toLowerCase();
    return lowercaseStr1 === lowercaseStr2;
  }

export const stringNotEquals = (str1: string, str2: string) => {
    const lowercaseStr1 = str1.toLowerCase();
    const lowercaseStr2 = str2.toLowerCase();
    return lowercaseStr1 !== lowercaseStr2;
  }