# Changelog

### [2.1.1](https://www.github.com/andrewesweet/FullMonty/compare/v2.1.0...v2.1.1) (2021-06-30)


### Bug Fixes

* Resolved "Could not find ExcelDna.Registration" error when loading the add-in ([eeeec06](https://www.github.com/andrewesweet/FullMonty/commit/eeeec06b34b16cab3c304740302deb1aee4635be))

## [2.1.0](https://www.github.com/andrewesweet/FullMonty/compare/v2.0.7...v2.1.0) (2021-06-29)


### Features

* Added ProductDistribution; a distribution whose samples as the product of samples taken from a list of other distributions ([f6b97e4](https://www.github.com/andrewesweet/FullMonty/commit/f6b97e4e5f59847179bfa4c96d52215d6fea9d55))

### [2.0.7](https://www.github.com/andrewesweet/FullMonty/compare/v2.0.6...v2.0.7) (2021-06-29)


### Bug Fixes

* The Add-In is now uploaded as a release asset ([d860438](https://www.github.com/andrewesweet/FullMonty/commit/d860438f840f78e5ab13dc9b6f713a669f9c6226))

### [2.0.6](https://www.github.com/andrewesweet/FullMonty/compare/v2.0.5...v2.0.6) (2021-06-29)


### Bug Fixes

* The Add-In is now uploaded as a release asset ([0409b73](https://www.github.com/andrewesweet/FullMonty/commit/0409b73949f52fb710f6ee54afc6c0b70275fde8))

### [2.0.5](https://www.github.com/andrewesweet/FullMonty/compare/v2.0.4...v2.0.5) (2021-06-29)


### Bug Fixes

* The Add-In is now uploaded as a release asset ([4fbbb3f](https://www.github.com/andrewesweet/FullMonty/commit/4fbbb3fe2ab730c84338bae869ea6188d398fb84))

### [2.0.4](https://www.github.com/andrewesweet/FullMonty/compare/v2.0.3...v2.0.4) (2021-06-29)


### Bug Fixes

* The Add-In is now uploaded as a release asset ([b3ea574](https://www.github.com/andrewesweet/FullMonty/commit/b3ea574bae6bc5137546d7e6aa6970bb7eb455b4))

### [2.0.3](https://www.github.com/andrewesweet/FullMonty/compare/v2.0.2...v2.0.3) (2021-06-29)


### Bug Fixes

* The Add-In is now uploaded as a release asset ([139c556](https://www.github.com/andrewesweet/FullMonty/commit/139c556df7f645f6b38c622cf9e022d87a0e1592))

### [2.0.2](https://www.github.com/andrewesweet/FullMonty/compare/v2.0.1...v2.0.2) (2021-06-29)


### Bug Fixes

* The Add-In is now uploaded as a release asset ([3f3554c](https://www.github.com/andrewesweet/FullMonty/commit/3f3554c8736fd1d856483f5973cd4e96978d0afe))

### [2.0.1](https://www.github.com/andrewesweet/FullMonty/compare/v2.0.0...v2.0.1) (2021-06-29)


### Bug Fixes

* The Add-In is now uploaded as a release asset ([291480a](https://www.github.com/andrewesweet/FullMonty/commit/291480aab03fff5a8590f9f664b5694aa9be5408))
* The Add-In is now uploaded as a release asset ([2b5f1b3](https://www.github.com/andrewesweet/FullMonty/commit/2b5f1b3757480487b3552f24333f57a292b91218))

## [2.0.0](https://www.github.com/andrewesweet/FullMonty/compare/v1.0.1...v2.0.0) (2021-06-29)


### ⚠ BREAKING CHANGES

* Removed named/not named variants of constructing a distribution. If no name is specified, one will be generated.
* UniformDistribution is now DiscreteUniformDistribution

### Features

* Added ContinuousUniformDistribution ([b11495d](https://www.github.com/andrewesweet/FullMonty/commit/b11495da7d4108014f917ea19fac93b7d6d7c711))
* Added xl_addin_debug artifact for debugging the packed add-in ([07d04c6](https://www.github.com/andrewesweet/FullMonty/commit/07d04c6a73b8aed69ad1dbbdbc7d7b1df960de70))
* Now targetting .NET 4.8 ([cf4e6b9](https://www.github.com/andrewesweet/FullMonty/commit/cf4e6b98365d26b976e8d8c5301be91eee55cb6f))
* UniformDistribution is now DiscreteUniformDistribution ([645a649](https://www.github.com/andrewesweet/FullMonty/commit/645a64997893cbfaf281e12d4080eca220a589de))


### Bug Fixes

* bumped System.Runtime.CompilerServices.Unsafe version to 4.7.1 ([ea20e8c](https://www.github.com/andrewesweet/FullMonty/commit/ea20e8cd23055ff25748dc9f0b91123677d752ce))
* Resolved "could not load assembly System.Runtime.CompilerServices.Unsafe" error ([e242e5b](https://www.github.com/andrewesweet/FullMonty/commit/e242e5b7bf8888ff794399cd132bbb3336f33c3a))
* Upload Add-Ins as release assets ([db437cd](https://www.github.com/andrewesweet/FullMonty/commit/db437cd9b02e834bc5d622d8b8961d708e406f7f))


### Code Refactoring

* Removed named/not named variants of constructing a distribution. If no name is specified, one will be generated. ([e5cd55c](https://www.github.com/andrewesweet/FullMonty/commit/e5cd55c8a304c5358f789a9650d08704eba96ed7))

### [1.0.1](https://www.github.com/andrewesweet/FullMonty/compare/v1.0.0...v1.0.1) (2021-06-28)


### Bug Fixes

* Pass secrets to release-please ([75c7542](https://www.github.com/andrewesweet/FullMonty/commit/75c754203551c95601576f55fe2022dd8d28abc8))

## 1.0.0 (2021-06-28)


### Features

* Create Beta, Normal, Sampled and Uniform distributions, take samples from a distribution, display samples and sum samples. ([c95f858](https://www.github.com/andrewesweet/FullMonty/commit/c95f858ae2d97e1ad4439b1f78c9dcac85633242))
