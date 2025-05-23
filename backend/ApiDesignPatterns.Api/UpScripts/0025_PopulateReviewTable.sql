DO
$$
    BEGIN
        INSERT INTO public.reviews (product_id, review_rating, review_text)
        VALUES (1, 4.5, 'Amazing product, exceeded my expectations!'),
               (2, 3.0, 'Decent, but could be improved in some areas.'),
               (3, 2.0, 'Not what I expected, quality was lower than expected.'),
               (4, 5.0, 'Great value for the price!'),
               (5, 1.0, 'Terrible experience, I would not recommend this product.'),
               (6, 4.0, 'Works as expected, no complaints.'),
               (7, 5.0, 'Highly recommended, will buy again!'),
               (8, 2.0, 'Not worth the money, felt like a cheap knockoff.'),
               (9, 5.0, 'Absolutely love it, five stars!'),
               (10, 3.5, 'Good quality, but delivery took too long.'),
               (11, 1.0, 'Product was damaged on arrival, had to return it.'),
               (12, 4.8, 'Solid choice for anyone looking for this type of product.'),
               (13, 3.7, 'The color didn’t match the picture, but otherwise it’s fine.'),
               (14, 5.0, 'Super easy to use, even for beginners.'),
               (15, 2.5, 'Did not meet my needs, returned for a refund.'),
               (16, 4.0, 'Affordable and good quality, very happy with it.'),
               (17, 1.0, 'Received a defective product, support was unhelpful.'),
               (18, 5.0, 'This is the best purchase I’ve made in a long time!'),
               (19, 3.2, 'Lacks important features, but it’s okay for the price.'),
               (20, 2.0, 'Came with missing parts, had to request replacements.'),
               (21, 1.5, 'Quality control needs improvement, lots of issues with this one.'),
               (22, 4.9, 'Excellent product and fantastic customer service!'),
               (23, 2.3, 'Cheap and does the job, but not for long-term use.'),
               (1, 4.1, 'Surprisingly good for the price, exceeded expectations.'),
               (2, 4.5, 'Super fast delivery and excellent product quality.'),
               (3, 3.0, 'It works, but it could have been designed better.'),
               (4, 5.0, 'Unbelievable product, changed my life!'),
               (5, 2.0, 'Didn’t live up to the hype, unfortunately.'),
               (6, 4.7, 'Very happy with this purchase, performs as expected.'),
               (7, 3.8, 'Good product, but the packaging was damaged.'),
               (8, 2.5, 'Mediocre at best, wouldn’t buy it again.'),
               (9, 4.0, 'Met my expectations but didn’t exceed them.'),
               (10, 1.5, 'Had high hopes, but it let me down.'),
               (11, 4.9, 'I would definitely buy this again, it’s that good.'),
               (12, 3.0, 'It’s okay, not great, but not terrible.'),
               (13, 4.6, 'Love it! I bought one for my friend too.'),
               (14, 2.2, 'Disappointed with the performance, wouldn’t recommend.'),
               (15, 5.0, '10/10, absolutely fantastic product!'),
               (16, 1.7, 'Had to return it, just didn’t meet my needs.'),
               (17, 3.5, 'It’s fine, but there are better alternatives.'),
               (18, 4.4, 'Exceeded my expectations in every way.'),
               (19, 4.2, 'Happy with the purchase, no issues so far.'),
               (20, 2.8, 'Felt overpriced for what it offers.'),
               (21, 4.8, 'Flawless experience, from purchase to delivery.'),
               (22, 5.0, 'Perfect product, couldn’t be happier.'),
               (23, 3.1, 'It’s okay, but nothing to write home about.'),
               (1, 2.5, 'Not as advertised, felt a bit scammed.'),
               (2, 5.0, 'Absolutely worth every penny.'),
               (3, 3.9, 'Pretty good, but I’ve seen better for the price.'),
               (4, 4.3, 'Delivered quickly, and it works as advertised.'),
               (5, 4.6, 'Best in its class, hands down.'),
               (6, 2.0, 'It broke after a week of use, total disappointment.'),
               (7, 4.1, 'No issues, would recommend to a friend.'),
               (8, 4.5, 'Easy to set up, no technical knowledge required.'),
               (9, 4.2, 'Better than I expected, especially for the price.'),
               (10, 1.2, 'Had issues right out of the box, had to get a replacement.'),
               (11, 5.0, 'I can’t believe how well this works, just amazing!'),
               (12, 3.3, 'Does the job, but with a few quirks.'),
               (13, 4.7, 'Exceeded my expectations, would buy again.'),
               (14, 4.0, 'Overall happy with it, no major complaints.'),
               (15, 2.6, 'Does not work as advertised, avoid this one.'),
               (16, 3.8, 'Good option if you’re on a tight budget.'),
               (17, 4.2, 'I was skeptical, but it works great!'),
               (18, 4.5, 'Lives up to the hype, amazing product!'),
               (19, 3.4, 'It’s good, but I expected more for the price.'),
               (20, 5.0, 'This is the gold standard of products!'),
               (21, 4.9, 'Excellent performance and build quality.'),
               (22, 2.3, 'Felt cheap, definitely not premium.'),
               (23, 4.5, 'Pleasantly surprised, better than I thought.'),
               (1, 4.8, 'Nothing to complain about, everything works perfectly.'),
               (2, 3.1, 'It’s fine, but you might find a better option.'),
               (3, 2.7, 'Not bad, but could have been better.'),
               (4, 4.0, 'Met expectations but didn’t exceed them.'),
               (5, 5.0, 'A must-have product, highly recommend it.'),
               (6, 3.9, 'Decent quality, but I found a better alternative later.');
    END
$$;
