import { ErrorMessage, Input, Select } from '../ProductForm.styles.ts';
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
        <Select id="ageGroup" {...register('ageGroup')}>
          <option value="">Select age group</option>
          <option value="puppy">Puppy</option>
          <option value="adult">Adult</option>
          <option value="senior">Senior</option>
        </Select>
        {errors.ageGroup && <ErrorMessage>{errors.ageGroup.message}</ErrorMessage>}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="breedSize">
          Breed Size
        </label>
        <Select id="breedSize" {...register('breedSize')}>
          <option value="">Select breed size</option>
          <option value="small">Small</option>
          <option value="medium">Medium</option>
          <option value="large">Large</option>
        </Select>
        {errors.breedSize && <ErrorMessage>{errors.breedSize.message}</ErrorMessage>}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="ingredients">
          Ingredients
        </label>
        <Input id="ingredients" {...register('ingredients')} />
        {errors.ingredients && <ErrorMessage>{errors.ingredients.message}</ErrorMessage>}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="storageInstructions">
          Storage Instructions
        </label>
        <Input id="storageInstructions" {...register('storageInstructions')} />
        {errors.storageInstructions && (
          <ErrorMessage>{errors.storageInstructions.message}</ErrorMessage>
        )}
      </div>

      <div className={'l-constrained'}>
        <label className={'form-label'} htmlFor="weightKg">
          Weight (kg)
        </label>
        <Input
          type="number"
          id="weightKg"
          min="0"
          step="0.01"
          {...register('weightKg', { valueAsNumber: true })}
        />
        {errors.weightKg && <ErrorMessage>{errors.weightKg.message}</ErrorMessage>}
      </div>
    </>
  );
};
