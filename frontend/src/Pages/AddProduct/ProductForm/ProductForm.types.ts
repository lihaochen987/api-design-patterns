import { components } from '../../../Shared/types';
import { z } from 'zod';

export type CreateProductRequest = components['schemas']['CreateProductRequest'];
export type CreateProductResponse = components['schemas']['CreateProductResponse'];

export const productBaseSchema = z.object(
  {
    name: z
      .string({
        errorMap: () => ({ message: 'Product name must be text' }),
      })
      .min(1, 'Product name is required'),

    pricing: z.object(
      {
        basePrice: z
          .number({
            errorMap: () => ({ message: 'Base price must be a number' }),
          })
          .positive('Base price must be positive'),

        discountPercentage: z
          .number({
            errorMap: () => ({ message: 'Discount percentage must be a number' }),
          })
          .positive('Discount percentage must be positive')
          .max(100, 'Discount percentage cannot exceed 100%'),

        taxRate: z
          .number({
            errorMap: () => ({ message: 'Tax rate must be a number' }),
          })
          .positive('Tax rate must be positive')
          .max(100, 'Tax rate cannot exceed 100%'),
      },
      {
        errorMap: () => ({ message: 'All pricing information is required' }),
      }
    ),

    dimensions: z.object(
      {
        width: z
          .number({
            errorMap: () => ({ message: 'Width must be a number' }),
          })
          .positive('Width must be positive')
          .max(50, 'Width cannot exceed 50 units'),

        height: z
          .number({
            errorMap: () => ({ message: 'Height must be a number' }),
          })
          .positive('Height must be positive')
          .max(50, 'Height cannot exceed 50 units'),

        length: z
          .number({
            errorMap: () => ({ message: 'Length must be a number' }),
          })
          .positive('Length must be positive')
          .max(100, 'Length cannot exceed 100 units'),
      },
      {
        errorMap: () => ({ message: 'All dimension information is required' }),
      }
    ),
  },
  {
    errorMap: () => ({ message: 'Product information is incomplete or invalid' }),
  }
);

export const otherProductSchema = productBaseSchema.extend({
  category: z.enum([
    'toys',
    'collarsAndLeashes',
    'beds',
    'feeders',
    'travelAccessories',
    'clothing',
  ]),
});

export const groomingAndHygieneSchema = productBaseSchema.extend({
  category: z.literal('groomingAndHygiene'),
  isNatural: z.boolean(),
  isHypoAllergenic: z.boolean(),
  usageInstructions: z.string().min(1),
  isCrueltyFree: z.boolean(),
  safetyWarnings: z.string().min(1),
});

export const petFoodSchema = productBaseSchema.extend({
  category: z.literal('petFood'),
  ageGroup: z.enum(['puppy', 'adult', 'senior']),
  breedSize: z.enum(['small', 'medium', 'large']),
  ingredients: z.string().min(1),
  nutritionalInfo: z.object({ key: z.string(), value: z.string() }).optional(),
  storageInstructions: z.string().min(1),
  weightKg: z.number().min(1),
});

export const productSchema = z.discriminatedUnion('category', [
  petFoodSchema,
  groomingAndHygieneSchema,
  otherProductSchema,
]);

export type ProductFormData = z.infer<typeof productSchema>;
