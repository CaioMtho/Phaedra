using AutoMapper;

using System.Reflection;

namespace Phaedra.Server.Mappings;

public class DomainToDtoMappingProfile : Profile
{
    public DomainToDtoMappingProfile()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var domainTypes = assembly.GetTypes().Where(t => 
            t.Namespace != null &&
            t.Namespace.StartsWith("Phaedra.Server.Models.Entities") && 
            (t.Namespace.EndsWith(".Users") || t.Namespace.EndsWith(".Projects") || t.Namespace.EndsWith(".Documents"))
        );

        var dtoTypes = assembly.GetTypes().Where(t => 
            t.Namespace != null &&
            t.Namespace.StartsWith("Phaedra.Server.Models.DTO")
        );

        foreach (var domainType in domainTypes)
        {
            // Procurar o DTO correspondente com base no padrão {Nome da Classe Original}Dto
            var correspondingDto = dtoTypes.FirstOrDefault(dto => dto.Name == $"{domainType.Name}Dto");
            if (correspondingDto == null) continue;
            // Criar mapeamento de domínio para DTO
            CreateMap(domainType, correspondingDto);
            // Criar mapeamento inverso (se necessário)
            CreateMap(correspondingDto, domainType);
        }
    }
}