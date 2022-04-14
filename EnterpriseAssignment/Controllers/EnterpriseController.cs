using EnterpriseAssignment.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAssignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnterpriseController : Controller
    {
        private readonly quiz_dbContext context;

        public EnterpriseController(quiz_dbContext context)
        {
            this.context = context;
        }

        [HttpGet] // attribute
        [Route("categories")]
        public IActionResult GetCategories()
        {
            var categories = context.Categories.Include(c => c.QuestionList);
            var categoryList = new List<CategoryWithQuestion>();
            foreach (var category in categories)
            {
                categoryList.Add(new CategoryWithQuestion(category, category.QuestionList.Count));
            }
            return Ok(categoryList);
        }

        [HttpPut]
        [Route("sessions")]
        public IActionResult CreateSession(string _username, int _categoryId)
        {
            // Get database table
            var dbSessions = context.Sessions;

            // Get answer from http
            Session mySession = new Session();
            mySession.Name = _username;
            mySession.Score = 0;
            mySession.Idcategory = _categoryId;

            // Add to table
            dbSessions.Add(mySession);

            // Save changes
            context.SaveChanges();

            // Return
            return Ok(mySession);
        }

        [HttpGet] // attribute
        [Route("question")]
        public IActionResult GetQuestion(int _categoryId, int _questionIndex)
        {
            // Get question
            var questions = context.Questions.Where(q => q.Idcategory == _categoryId).ToList();
            for (int i = 0; i < questions.Count; i++)
            {
                Question question = questions[i];
                if (i == _questionIndex-1)
                {
                    return Ok(question);
                }
            }

            // Question not found
            return Ok(null);
        }

        [HttpPut]
        [Route("answers")]
        public IActionResult PostAnswer(string _answerCharacter, int _idQuestion, int _idCategory, int _idUser)
        {
            // Get database table
            var dbAnswers = context.Answers;

            // Get answer from http
            Answer myAnswer = new Answer();
            myAnswer.AnswerCharacter = _answerCharacter;
            myAnswer.Idanswer = 0;
            myAnswer.Idquestion = _idQuestion;
            myAnswer.Idcategory = _idCategory;
            myAnswer.Iduser = _idUser;

            // Add to table
            dbAnswers.Add(myAnswer);

            // Save changes
            context.SaveChanges();

            // Return
            return Ok(dbAnswers);
        }

        [HttpPatch] // attribute
        [Route("score")]
        public IActionResult UpdateHighScore(int _idUser)
        {
            // Get tables
            var dbAnswers = context.Answers.ToList<Answer>();
            var dbQuestions = context.Questions.ToList<Question>();
            var dbSessions = context.Sessions.ToList<Session>();

            // Compare and increment score
            int score = 0;
            foreach (var answer in dbAnswers)
            {
                foreach (var question in dbQuestions)
                {
                    if (answer.Iduser == _idUser &&
                        answer.Idquestion == question.Idquestion &&
                        answer.Idcategory == question.Idcategory &&
                        answer.AnswerCharacter == question.CorrectAnswer)
                    {
                        score++;
                    }
                }
            }

            // Update session score
            foreach (Session session in dbSessions)
            {
                if (session.Iduser == _idUser)
                {
                    session.Score = score;
                }
            }

            // Save
            context.SaveChanges();

            // Return
            return Ok(score);
        }

        [HttpGet] // attribute
        [Route("highscore")]
        public IActionResult GetHighScores() // Has more techniques
        {
            // Get tables
            var dbSessions = context.Sessions.OrderByDescending(session => session.Score).ToList();

            // Return
            return Ok(dbSessions);
        }

        [HttpPatch] // attribute
        [Route("highscore")]
        public IActionResult UpdateHighScoresAlternate(int _idUser) // Has more techniques
        {
            // Get tables
            var dbAnswers = context.Answers.Where(a => a.Iduser == _idUser).Select(a => new AnswerWithQuestion(a.AnswerCharacter, a.Idquestion)).ToList();

            var dbSessions = context.Sessions.FirstOrDefault(s => s.Iduser == _idUser);
            var sessionOrder = context.Sessions.OrderBy(s => s.Iduser);

            // Compare and increment score
            int score = 0;
            foreach (var answer in dbAnswers)
            {
                var question = context.Questions.FirstOrDefault(q => q.Idquestion == answer.IdQuestion);

                if (answer.AnswerCharacter == question.CorrectAnswer)
                {
                    score++;
                }
            }

            // Update session score

            dbSessions.Score = score;


            // Save
            context.SaveChanges();

            // Return
            return Ok(score);
        }

        [HttpGet]
        [Route("sessions")]
        public IActionResult PostUserId(string myString)
        {
            var userIds = context.Sessions.ToList();
            return Ok(userIds);
        }
    }

    // making a model
    public record CategoryWithQuestion(Category Categories, int QuestionCount);
    public record AnswerWithQuestion(string AnswerCharacter, int? IdQuestion); 
}
