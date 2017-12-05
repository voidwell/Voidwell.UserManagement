using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voidwell.UserManagement.Exceptions;
using Voidwell.UserManagement.Models;
using Voidwell.UserManagement.Data;
using Microsoft.AspNetCore.Identity;

namespace Voidwell.UserManagement.Services
{
    public class SecurityQuestionService : ISecurityQuestionService
    {
        private readonly Func<UserDbContext> _dbContextFactory;
        private readonly UserManager<Data.Models.ApplicationUser> _userManager;
        private readonly ILogger<SecurityQuestionService> _logger;

        public SecurityQuestionService(Func<UserDbContext> dbContextFactory, UserManager<Data.Models.ApplicationUser> userManager,
            ILogger<SecurityQuestionService> logger)
        {
            _dbContextFactory = dbContextFactory;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IEnumerable<SecurityQuestion>> GetSecurityQuestions(Guid userId)
        {
            var dbContext = _dbContextFactory();

            var questions = await dbContext.SecurityQuestions.Where(a => a.UserId == userId)
                .ToListAsync();

            if (questions == null)
                return null;

            return questions.Select(q => new SecurityQuestion { Question = q.Question, Answer = q.Answer });
        }

        public async Task<IEnumerable<SecurityQuestion>> CreateSecurityQuestions(Guid userId, IEnumerable<SecurityQuestion> securityQuestions)
        {
            var dbQuestions = securityQuestions.Select(q =>
            {
                return new Data.Models.SecurityQuestion
                {
                    UserId = userId,
                    Question = q.Question,
                    Answer = q.Answer
                };
            });

            var dbContext = _dbContextFactory();

            dbContext.SecurityQuestions.AddRange(dbQuestions);
            await dbContext.SaveChangesAsync();

            return securityQuestions;
        }

        public async Task RemoveSecurityQuestions(Guid userId)
        {
            var dbContext = _dbContextFactory();

            var questions = await dbContext.SecurityQuestions.Where(q => q.UserId == userId)
                .ToListAsync();

            if (questions == null)
                return;

            dbContext.SecurityQuestions.RemoveRange(questions);
            await dbContext.SaveChangesAsync();
        }

        public IEnumerable<string> GetSecurityQuestionsList()
        {
            return SecurityQuestionsList;
        }

        public void ValidateSecurityQuestions(IEnumerable<SecurityQuestion> questions)
        {
            var allQuestions = GetSecurityQuestionsList();

            foreach(var question in questions)
            {
                if (!questions.Contains(question))
                {
                    _logger.LogWarning($"Invalid security question \"{question}\" was used");
                    throw new InvalidSecurityQuestionException();
                }
            }
        }

        public async Task<IEnumerable<SecurityQuestion>> GetSecurityQuestionsByEmail(string email)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower());

            return await GetSecurityQuestions(user.Id);
        }

        public static readonly string[] SecurityQuestionsList =
        {
            "What Is your favorite book?",
            "What is the name of the road you grew up on?",
            "What is your mother’s maiden name?",
            "What was the name of your first pet?",
            "What was the first company that you worked for?",
            "Where did you meet your spouse?",
            "Where did you go to high school?",
            "Where did you go to college?",
            "What is your favorite food?",
            "What city were you born in?",
            "Where is your favorite place to vacation?",
            "What is the first and last name of your first boyfriend or girlfriend?",
            "Who is your favorite actor, musician, or artist?",
            "What is your favorite movie?",
            "What was the make of your first car?",
            "What is your favorite color?",
            "What is your father's middle name?",
            "What is the name of your first grade teacher?",
            "What was your high school mascot?",
            "What was the name of the hospital where you were born?",
            "What was your childhood nickname?",
            "In what town or city was your first job?"
        };
    }
}
