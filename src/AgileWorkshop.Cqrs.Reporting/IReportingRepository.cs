﻿namespace AgileWorkshop.Cqrs.Reporting
{
	using System.Collections.Generic;

	public interface IReportingRepository 
	{
		IEnumerable<TDto> GetByExample<TDto>(object example) where TDto : class;
		void Save<TDto>(TDto dto) where TDto : class;
		void Update<TDto>(object update, object where) where TDto : class;
		void Delete<TDto>(object example) where TDto : class;
	}
}