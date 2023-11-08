using FluentValidation;

namespace MovieAPI.Models;

public class MovieDtoValidator : AbstractValidator<MovieDto>
{
    public MovieDtoValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(request => request.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(request => request.CountryId).NotEmpty().WithMessage("CountryId is required.");
        RuleFor(request => request.ReleaseDate).NotEmpty().WithMessage("ReleaseDate is required.");
        RuleFor(request => request.GenresIds).NotEmpty().WithMessage("GenresIds must not be empty.");
        RuleFor(request => request.PeopleIds).NotEmpty().WithMessage("PeopleIds must not be empty.");
        RuleFor(request => request.Poster).NotEmpty().WithMessage("Poster file is required.");
    }
}