import { useFormContext } from 'react-hook-form';
import { petFoodSchema } from '../ProductForm.types.ts';
import { z } from 'zod';

export const PetFoodForm = () => {
  const {
    register,
    formState: { errors },
  } = useFormContext<z.infer<typeof petFoodSchema>>();

  return (
    <>
      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="ageGroup">
          Age Group
        </label>
        <select className={'form-select'} id="ageGroup" {...register('ageGroup')}>
          <option value="">Select age group</option>
          <option value="puppy">Puppy</option>
          <option value="adult">Adult</option>
          <option value="senior">Senior</option>
        </select>
        {errors.ageGroup && <p className={'form-error-message'}>{errors.ageGroup.message}</p>}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="breedSize">
          Breed Size
        </label>
        <select className={'form-select'} id="breedSize" {...register('breedSize')}>
          <option value="">Select breed size</option>
          <option value="small">Small</option>
          <option value="medium">Medium</option>
          <option value="large">Large</option>
        </select>
        {errors.breedSize && <p className={'form-error-message'}>{errors.breedSize.message}</p>}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="ingredients">
          Ingredients
        </label>
        <input className={'form-input'} id="ingredients" {...register('ingredients')} />
        {errors.ingredients && <p className={'form-error-message'}>{errors.ingredients.message}</p>}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="storageInstructions">
          Storage Instructions
        </label>
        <input
          className={'form-input'}
          id="storageInstructions"
          {...register('storageInstructions')}
        />
        {errors.storageInstructions && (
          <p className={'form-error-message'}>{errors.storageInstructions.message}</p>
        )}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="weightKg">
          Weight (kg)
        </label>
        <input
          className={'form-input'}
          type="number"
          id="weightKg"
          min="0"
          step="0.01"
          {...register('weightKg', { valueAsNumber: true })}
        />
        {errors.weightKg && <p className={'form-error-message'}>{errors.weightKg.message}</p>}
      </div>
    </>
  );
};
