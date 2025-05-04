export const isEmpty = (str: string | null | undefined): boolean => {
    if (str === null || str === undefined) {
        return true;
    }
    return str.trim().length === 0;
}

export const upperCaseFirstLetter = (str: string): string => {
    if (isEmpty(str)) {
        return str;
    }
    return str.charAt(0).toUpperCase() + str.slice(1);
}

export const upperCase = (str: string): string => {
    if (isEmpty(str)) {
        return str;
    }
    return str.toUpperCase();
}