import { ErrorMessage, FormGroup, Input, Label, Select } from '../ProductForm.styles.ts';
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
      <FormGroup>
        <Label htmlFor="ageGroup">Age Group</Label>
        <Select id="ageGroup" {...register('ageGroup')}>
          <option value="">Select age group</option>
          <option value="puppy">Puppy</option>
          <option value="adult">Adult</option>
          <option value="senior">Senior</option>
        </Select>
        {errors.ageGroup && <ErrorMessage>{errors.ageGroup.message}</ErrorMessage>}
      </FormGroup>

      <FormGroup>
        <Label htmlFor="breedSize">Breed Size</Label>
        <Select id="breedSize" {...register('breedSize')}>
          <option value="">Select breed size</option>
          <option value="small">Small</option>
          <option value="medium">Medium</option>
          <option value="large">Large</option>
        </Select>
        {errors.breedSize && <ErrorMessage>{errors.breedSize.message}</ErrorMessage>}
      </FormGroup>

      <FormGroup>
        <Label htmlFor="ingredients">Ingredients</Label>
        <Input id="ingredients" {...register('ingredients')} />
        {errors.ingredients && <ErrorMessage>{errors.ingredients.message}</ErrorMessage>}
      </FormGroup>

      <FormGroup>
        <Label htmlFor="storageInstructions">Storage Instructions</Label>
        <Input id="storageInstructions" {...register('storageInstructions')} />
        {errors.storageInstructions && (
          <ErrorMessage>{errors.storageInstructions.message}</ErrorMessage>
        )}
      </FormGroup>

      <FormGroup>
        <Label htmlFor="weightKg">Weight (kg)</Label>
        <Input
          type="number"
          id="weightKg"
          min="0"
          step="0.01"
          {...register('weightKg', { valueAsNumber: true })}
        />
        {errors.weightKg && <ErrorMessage>{errors.weightKg.message}</ErrorMessage>}
      </FormGroup>
    </>
  );
};
