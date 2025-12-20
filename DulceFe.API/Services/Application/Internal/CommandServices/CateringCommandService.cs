using DulceFe.API.Services.Domain.Model.Aggregates;
using DulceFe.API.Services.Domain.Repositories;
using DulceFe.API.Services.Domain.Services;
using DulceFe.API.Shared.Domain.Repositories;

namespace DulceFe.API.Services.Application.Internal.CommandServices;

public class CateringCommandService : ICateringInquiryCommandService
{
    private readonly ICateringInquiryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CateringCommandService(ICateringInquiryRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(int id, string status)
    {
        var inquiry = await _repository.FindByIdAsync(id);
        if (inquiry == null) throw new Exception("Inquiry not found");

        inquiry.Status = status;
        _repository.Update(inquiry);
        await _unitOfWork.CompleteAsync();
    }
}
