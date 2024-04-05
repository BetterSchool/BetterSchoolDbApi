using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Reflection;

namespace BetterAdminDbAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PupilController : ControllerBase
    {
        // GET: api/<PupilController>
        [HttpGet("GetPupils")]
        public HttpStatusCode Get()
        {
            //List<Pupil> pupils = AddressRepo.GetAll();
            if (pupils.Count < 0){
                return NotFound();
            }
                return pupils;
        }

        // GET api/<PupilController>/5
        [HttpGet("GetPupilById")]
        public Address GetPupilById([FromBody] int id)
        {
            //Pupil? pupil = PupilRepo.GetById(id);
            if (pupil == null){
            return NotFound();
            }
        
        return pupil;
        }
    

        // POST api/<PupilController>
        [HttpPost]
        public HttpStatusCode Post([FromBody]Pupil pupil)
        {
            //The order of the executed code is important, as the code is requesting the use of the database, which has it's own constraints and error handling.

            //Check model state
            if (!ModelState.IsValid)
                return HttpStatusCode.UnprocessableEntity; //https://stackoverflow.com/questions/3050518/what-http-status-response-code-should-i-use-if-the-request-is-missing-a-required

            //Create person and address first
            Person person = new Person()
            {
                FirstName = pupilDTO.FirstName,
                LastName = pupilDTO.LastName,
                PhoneNo = pupilDTO.PhoneNumber,
                Email = pupilDTO.Email
            };
            dbContext.People.Add(person);
            await dbContext.SaveChangesAsync();

            Address address = new Address()
            {
                City = pupilDTO.City,
                Road = pupilDTO.Road,
                PostalCode = pupilDTO.PostalCode,
            };
            dbContext.Addresses.Add(address);
            await dbContext.SaveChangesAsync();

            //Create guardian if: bool guardian, is true
            if (pupilDTO.GuardDTO != null)
            {
                //Guardian consists of two foreign keys FK_Guardian_Person(ID) & FK_Guardian_Address(ID), so they must be created before those fields can be bound
                Address newGuardianAddress = new Address()
                {
                    City = pupilDTO.GuardDTO.City,
                    Road = pupilDTO.GuardDTO.Road,
                    PostalCode = pupilDTO.GuardDTO.PostalCode,
                };
                dbContext.Addresses.Add(newGuardianAddress);
                await dbContext.SaveChangesAsync();

                Person newGuardianPerson = new Person()
                {
                    FirstName = pupilDTO.GuardDTO.FirstName,
                    LastName = pupilDTO.GuardDTO.LastName,
                    PhoneNo = pupilDTO.GuardDTO.PhoneNumber,
                    Email = pupilDTO.GuardDTO.Email
                };
                dbContext.People.Add(newGuardianPerson);
                await dbContext.SaveChangesAsync();

                //Retrieve address of guardian to get newly created AddressId
                Address guardianAddress = (Address)(from db in dbContext.Addresses
                                                    where 
                                                    db.City == pupilDTO.City && 
                                                    db.Road == pupilDTO.Road && 
                                                    db.PostalCode == pupilDTO.PostalCode
                                                    select db);

                //Retrieve newly created guardian(person) in database to get newly created PersonId
                Person guardianPerson = (Person)(from db in dbContext.People
                                                 where 
                                                 db.FirstName == pupilDTO.GuardDTO.FirstName && 
                                                 db.LastName == pupilDTO.GuardDTO.LastName && 
                                                 db.PhoneNo == pupilDTO.GuardDTO.PhoneNumber && 
                                                 db.Email == pupilDTO.GuardDTO.Email
                                                 select db);
                
                Guardian guard = new Guardian()
                {
                    WorkPhoneNo = pupilDTO.GuardDTO.WorkPhoneNo,
                    AddressId = guardianAddress.Id,
                    PersonId = guardianPerson.Id
                };

                dbContext.Guardians.Add(guard);
                await dbContext.SaveChangesAsync();
            }


            //Create pupil
            Pupil pupil = new Pupil()
            {
                MobileNo = pupilDTO.MobileNumber,
                DateOfBirth = pupilDTO.DateOfBirth,
                Gender = pupilDTO.Gender,
                EnrollmentDate = pupilDTO.EnrollmentDate,
                Note = pupilDTO.Note,
                PhotoPermission = pupilDTO.PhotoPermission,
                School = pupilDTO.School,
                Grade = pupilDTO.Grade,
                //TODO: PersonID
                //TODO: GuardianID
                //TODO: AddressID
            };
            dbContext.Pupils.Add(pupil);
            await dbContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        // PUT api/<PupilController>/5
        [HttpPut("UpdatePupil")]
        public async Task<HttpStatusCode> UpdatePupil([FromBody] PupilDTO changes)
        {
            Pupil? dbPupil = await dbContext.Pupils.FirstOrDefaultAsync(s => s.Id == changes.Id);
            if(dbPupil == null)
                return HttpStatusCode.NotFound;

            dbPupil.MobileNo = changes.MobileNumber;
            dbPupil.DateOfBirth = changes.DateOfBirth;
            dbPupil.Gender = changes.Gender;
            dbPupil.EnrollmentDate = changes.EnrollmentDate;
            dbPupil.Note = changes.Note;
            dbPupil.PhotoPermission = changes.PhotoPermission;
            dbPupil.School = changes.School;
            dbPupil.Grade = changes.Grade;
            
            if (changes.Road != null && changes.PostalCode != null && changes.City != null)
            {
                //Insert new address into db
                var addressNew = new Address()
                {
                    City = changes.City,
                    Road = changes.Road,
                    PostalCode = changes.PostalCode,
                };
                dbContext.Addresses.Add(addressNew);
                await dbContext.SaveChangesAsync();

                //Fetch new address for AddressId
                Address adressId = (Address)(from db in dbContext.Addresses
                                                    where
                                                    db.City == addressNew.City &&
                                                    db.Road == addressNew.Road &&
                                                    db.PostalCode == addressNew.PostalCode
                                                    select db);

                //Change AddressId to a new AddressId reference
                dbPupil.AddressId = adressId.Id;

                //Remove old address
                Address oldAddress = (Address)(from db in dbContext.Addresses
                                                    where
                                                    db.City == changes.City &&
                                                    db.Road == changes.Road &&
                                                    db.PostalCode == changes.PostalCode
                                                    select db);
                dbContext.Addresses.Attach(oldAddress);
                dbContext.Addresses.Remove(oldAddress);
                await dbContext.SaveChangesAsync();
            }

            //Repeat for guardian
            if (changes.GuardDTO != null)
            {
                //TODO: Check if guardian already exists in database

                //Insert new guardian address into db
                var addressNew = new Address()
                {
                    City = changes.GuardDTO.City,
                    Road = changes.GuardDTO.Road,
                    PostalCode = changes.GuardDTO.PostalCode,
                };
                dbContext.Addresses.Add(addressNew);

                //Insert new guardian person into db
                var personNew = new Person()
                {
                    FirstName = changes.GuardDTO.FirstName,
                    LastName = changes.GuardDTO.LastName,
                    PhoneNo = changes.GuardDTO.PhoneNumber,
                    Email = changes.GuardDTO.Email,
                };
                dbContext.People.Add(personNew);
                await dbContext.SaveChangesAsync();

                //Fetch new guardian address for AddressId
                Address addressId = (Address)(from db in dbContext.Addresses
                                             where
                                             db.City == addressNew.City &&
                                             db.Road == addressNew.Road &&
                                             db.PostalCode == addressNew.PostalCode
                                             select db);

                //Fetch new guardian person for PersonId
                Address personId = (Address)(from db in dbContext.People
                                             where
                                             db.FirstName == personNew.FirstName &&
                                             db.LastName == personNew.FirstName &&
                                             db.PhoneNo == personNew.PhoneNo &&
                                             db.Email == personNew.Email
                                             select db);

                //Make new guardian
                Guardian guardian = new Guardian()
                {
                    WorkPhoneNo = changes.GuardDTO.WorkPhoneNo,
                    AddressId = addressId.Id,
                    PersonId = personId.Id
                };

                Address newGuardian = (Address)(from db in dbContext.Guardians
                                             where
                                             db.WorkPhoneNo == guardian.WorkPhoneNo &&
                                             db.AddressId == guardian.AddressId &&
                                             db.PersonId == guardian.PersonId
                                             select db);

                int oldId = dbPupil.GuardianId;
                dbPupil.GuardianId = newGuardian.Id;

                //Remove old guardian
                Guardian oldGuardian = (Guardian)(from db in dbContext.Guardians
                                                  where
                                                  db.Id == oldId
                                                  select db);
                dbContext.Guardians.Attach(oldGuardian);
                dbContext.Guardians.Remove(oldGuardian);

                //Remove old address
                Address oldAddress = (Address)(from db in dbContext.Addresses
                                               where
                                               db.Id == dbPupil.AddressId
                                               select db);
                dbContext.Addresses.Attach(oldAddress);
                dbContext.Addresses.Remove(oldAddress);

                //Remove old person
                Person oldPerson = (Person)(from db in dbContext.People
                                               where
                                               db.Id == dbPupil.PersonId
                                               select db);
                dbContext.Addresses.Attach(oldAddress);
                dbContext.Addresses.Remove(oldAddress);


                await dbContext.SaveChangesAsync();
            }

            await dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        // DELETE api/<PupilController>/5
        [HttpGet("DeletePupilById")]
        public async Task<HttpStatusCode> Delete(int id)
        {
            var entity = new Pupil()
            {
                Id = id
            };
            dbContext.Pupils.Attach(entity);
            dbContext.Pupils.Remove(entity);
            await dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
