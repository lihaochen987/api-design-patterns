import { ErrorMessage, FormGroup, Input, Label, Select } from '../ProductForm.styles.ts';
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
      <FormGroup>
        <Label htmlFor="isNatural">Natural Product</Label>
        <Select
          id="isNatural"
          {...register('isNatural', { setValueAs: value => value === 'true' })}
        >
          <option value="true">Yes</option>
          <option value="false">No</option>
        </Select>
        {errors.isNatural && <ErrorMessage>{errors.isNatural.message}</ErrorMessage>}
      </FormGroup>

      <FormGroup>
        <Label htmlFor="isHypoAllergenic">Hypoallergenic</Label>
        <Select
          id="isHypoAllergenic"
          {...register('isHypoAllergenic', { setValueAs: value => value === 'true' })}
        >
          <option value="true">Yes</option>
          <option value="false">No</option>
        </Select>
        {errors.isHypoAllergenic && <ErrorMessage>{errors.isHypoAllergenic.message}</ErrorMessage>}
      </FormGroup>

      <FormGroup>
        <Label htmlFor="isCrueltyFree">Cruelty Free</Label>
        <Select
          id="isCrueltyFree"
          {...register('isCrueltyFree', { setValueAs: value => value === 'true' })}
        >
          <option value="true">Yes</option>
          <option value="false">No</option>
        </Select>
        {errors.isCrueltyFree && <ErrorMessage>{errors.isCrueltyFree.message}</ErrorMessage>}
      </FormGroup>

      <FormGroup>
        <Label htmlFor="usageInstructions">Usage Instructions</Label>
        <Input id="usageInstructions" {...register('usageInstructions')} />
        {errors.usageInstructions && (
          <ErrorMessage>{errors.usageInstructions.message}</ErrorMessage>
        )}
      </FormGroup>

      <FormGroup>
        <Label htmlFor="safetyWarnings">Safety Warnings</Label>
        <Input id="safetyWarnings" {...register('safetyWarnings')} />
        {errors.safetyWarnings && <ErrorMessage>{errors.safetyWarnings.message}</ErrorMessage>}
      </FormGroup>
    </>
  );
};
