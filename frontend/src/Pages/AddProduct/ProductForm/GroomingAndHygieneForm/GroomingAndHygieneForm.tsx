import { ErrorMessage, Input, Select } from '../ProductForm.styles.ts';
import { useFormContext } from 'react-hook-form';
import { z } from 'zod';
import { groomingAndHygieneSchema } from '../ProductForm.types.ts';

export const GroomingAndHygieneForm = () => {
  const {
    register,
    formState: { errors },
  } = useFormContext<z.infer<typeof groomingAndHygieneSchema>>();

  return (
    <>
      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="isNatural">
          Natural Product
        </label>
        <Select
          id="isNatural"
          {...register('isNatural', { setValueAs: value => value === 'true' })}
        >
          <option value="true">Yes</option>
          <option value="false">No</option>
        </Select>
        {errors.isNatural && <ErrorMessage>{errors.isNatural.message}</ErrorMessage>}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="isHypoAllergenic">
          Hypoallergenic
        </label>
        <Select
          id="isHypoAllergenic"
          {...register('isHypoAllergenic', { setValueAs: value => value === 'true' })}
        >
          <option value="true">Yes</option>
          <option value="false">No</option>
        </Select>
        {errors.isHypoAllergenic && <ErrorMessage>{errors.isHypoAllergenic.message}</ErrorMessage>}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="isCrueltyFree">
          Cruelty Free
        </label>
        <Select
          id="isCrueltyFree"
          {...register('isCrueltyFree', { setValueAs: value => value === 'true' })}
        >
          <option value="true">Yes</option>
          <option value="false">No</option>
        </Select>
        {errors.isCrueltyFree && <ErrorMessage>{errors.isCrueltyFree.message}</ErrorMessage>}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="usageInstructions">
          Usage Instructions
        </label>
        <Input id="usageInstructions" {...register('usageInstructions')} />
        {errors.usageInstructions && (
          <ErrorMessage>{errors.usageInstructions.message}</ErrorMessage>
        )}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="safetyWarnings">
          Safety Warnings
        </label>
        <Input id="safetyWarnings" {...register('safetyWarnings')} />
        {errors.safetyWarnings && <ErrorMessage>{errors.safetyWarnings.message}</ErrorMessage>}
      </div>
    </>
  );
};
