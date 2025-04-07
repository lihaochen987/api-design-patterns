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
        <select
          className={'form-select'}
          id="isNatural"
          {...register('isNatural', { setValueAs: value => value === 'true' })}
        >
          <option value="true">Yes</option>
          <option value="false">No</option>
        </select>
        {errors.isNatural && <p className={'form-error-message'}>{errors.isNatural.message}</p>}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="isHypoAllergenic">
          Hypoallergenic
        </label>
        <select
          className={'form-select'}
          id="isHypoAllergenic"
          {...register('isHypoAllergenic', { setValueAs: value => value === 'true' })}
        >
          <option value="true">Yes</option>
          <option value="false">No</option>
        </select>
        {errors.isHypoAllergenic && (
          <p className={'form-error-message'}>{errors.isHypoAllergenic.message}</p>
        )}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="isCrueltyFree">
          Cruelty Free
        </label>
        <select
          className={'form-select'}
          id="isCrueltyFree"
          {...register('isCrueltyFree', { setValueAs: value => value === 'true' })}
        >
          <option value="true">Yes</option>
          <option value="false">No</option>
        </select>
        {errors.isCrueltyFree && (
          <p className={'form-error-message'}>{errors.isCrueltyFree.message}</p>
        )}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="usageInstructions">
          Usage Instructions
        </label>
        <input className={'form-input'} id="usageInstructions" {...register('usageInstructions')} />
        {errors.usageInstructions && (
          <p className={'form-error-message'}>{errors.usageInstructions.message}</p>
        )}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="safetyWarnings">
          Safety Warnings
        </label>
        <input className={'form-input'} id="safetyWarnings" {...register('safetyWarnings')} />
        {errors.safetyWarnings && (
          <p className={'form-error-message'}>{errors.safetyWarnings.message}</p>
        )}
      </div>
    </>
  );
};
