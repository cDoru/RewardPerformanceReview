﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PerformanceReview.Data.EntityFramework.Entity;
using PerformanceReview.Data.EntityFramework.Repository;
using PerformanceReview.Web.Rest.Models;

namespace PerformanceReview.Web.Rest.Controllers
{
    [Route("api/employees/{employeeId}/employeereviews")]
    public class EmployeeReviewsController : Controller
    {
        private IPerformanceReviewRepository PerformanceReviewRepository { get; set; }

        public EmployeeReviewsController(IPerformanceReviewRepository performanceReviewRepository)
        {
            PerformanceReviewRepository = performanceReviewRepository;
        }

        [HttpGet()]
        public IActionResult GetEmployeeReviewsForEmployee(int employeeId)
        {
            IEnumerable<EmployeeReview> employeeReviewsForEmployeeFromRepo = null;

            if (!PerformanceReviewRepository.EmployeeExists(employeeId))
            {
                return NotFound();
            }

            try
            {
                employeeReviewsForEmployeeFromRepo = PerformanceReviewRepository.GetEmployeeReviewsForEmployee(employeeId);
            }
            catch (Exception)
            {
                throw new Exception($"Getting employee reviews for employee {employeeId} failed");
            }

            var employeeReviewsForEmployee = Mapper.Map<IEnumerable<EmployeeReviewDto>>(employeeReviewsForEmployeeFromRepo);

            return Ok(employeeReviewsForEmployee);
        }

        [HttpGet("{id}", Name = "GetEmployeeReview")]
        public IActionResult GetEmployeeReviewForEmployee(int employeeId, int id)
        {
            EmployeeReview employeeReviewForEmployeeFromRepo = null;

            if (!PerformanceReviewRepository.EmployeeExists(employeeId))
            {
                return NotFound();
            }

            try
            {
                employeeReviewForEmployeeFromRepo = PerformanceReviewRepository.GetEmployeeReviewForEmployee(employeeId, id);
            }
            catch (Exception)
            {
                throw new Exception($"Getting employee review for employee {employeeId} failed");
            }

            if (employeeReviewForEmployeeFromRepo == null)
            {
                return NotFound();
            }

            var employeeReviewForEmployee = Mapper.Map<EmployeeReviewDto>(employeeReviewForEmployeeFromRepo);
            return Ok(employeeReviewForEmployee);

        }

        [HttpPost()]
        public IActionResult CreateEmployeeReviewForEmployee(int employeeId, [FromBody] CreateEmployeeReviewDto employeeReview)
        {
            if (employeeReview == null)
            {
                return BadRequest();
            }

            if (!PerformanceReviewRepository.EmployeeExists(employeeId))
            {
                return NotFound();
            }


            var employeeReviewEntity = Mapper.Map<EmployeeReview>(employeeReview);
            employeeReviewEntity.EmployeeId = employeeId;

            try
            { 
                PerformanceReviewRepository.Insert(employeeReviewEntity);
            }
            catch (Exception)
            {
                throw new Exception($"Inserting employee review for employee {employeeId} failed on commit");
            }

            var newEmployeeReview = Mapper.Map<EmployeeReviewDto>(employeeReviewEntity);

            return CreatedAtRoute("GetEmployeeReview", new { id = newEmployeeReview.Id }, newEmployeeReview);

        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployeeReviewForEmployee(int employeeId, int id, [FromBody] UpdateEmployeeReviewDto employeeReview)
        {
            if(employeeReview == null)
            {
                return BadRequest();
            }

            if (!PerformanceReviewRepository.EmployeeExists(employeeId))
            {
                return NotFound();
            }

            var employeeReviewForEmployeeFromRepo = PerformanceReviewRepository.GetEmployeeReviewForEmployee(employeeId, id);

            if (employeeReviewForEmployeeFromRepo == null)
            {
                return NotFound();
            }

            Mapper.Map(employeeReview, employeeReviewForEmployeeFromRepo);

            try
            {
                PerformanceReviewRepository.Update(employeeReviewForEmployeeFromRepo);
            }
            catch (Exception)
            {
                throw new Exception($"Updating employee review {id} for employee {employeeId} failed on commit");
            }

            return NoContent();

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployeeReviewForEmployee(int employeeId, int id)
        {
            if (!PerformanceReviewRepository.EmployeeExists(employeeId))
            {
                return NotFound();
            }

            var employeeReviewForEmployeeFromRepo = PerformanceReviewRepository.GetEmployeeReviewForEmployee(employeeId, id);

            if (employeeReviewForEmployeeFromRepo == null)
            {
                return NotFound();
            }

            try
            {
                PerformanceReviewRepository.Delete(employeeReviewForEmployeeFromRepo);
            }
            catch (Exception)
            {
                throw new Exception($"Deleting employee review {id} for employee {employeeId} failed on commit");
            }

            return NoContent();
        }
    }
}
