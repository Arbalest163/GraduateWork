import { useState } from 'react';
import { ErrorValidation } from '../api/models/models';
import { stringEquals, stringNotEquals } from '../api/common/common-components';

const useValidationErrors = () => {
  const [validationErrors, setValidationErrors] = useState<ErrorValidation[]>([]);

  const isAnyValidationError = (propertyName: string): boolean => {
    return validationErrors.some((err) => stringEquals(err.propertyName, propertyName));
  };

  const clearValidationError = (propertyName?: string) => {
    if(propertyName) {
        const clearError = validationErrors.filter((err) => stringNotEquals(err.propertyName, propertyName));
        setValidationErrors(clearError);
    } else {
        setValidationErrors([]);
    }
  };

  return { validationErrors, setValidationErrors, isAnyValidationError, clearValidationError };
};

export default useValidationErrors;
