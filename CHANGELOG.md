# Changelog

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
