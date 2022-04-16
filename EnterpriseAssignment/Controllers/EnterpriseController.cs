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
            var questions = context.Questions.Where(q => q.Idcategory == _categoryId).ToList();
            var dbSessionQuestions = context.SessionQuestions;

            // Get answer from http
            Session mySession = new Session();
            mySession.Name = _username;
            mySession.Score = 0;
            mySession.Idcategory = _categoryId;
            mySession.CurrentQuestionIndex = 0;

            // Add to table
            dbSessions.Add(mySession);

            // Save changes
            context.SaveChanges();

            // Get session again now that it has a userId
            Session newSession = context.Sessions.Where(s => s.Name == _username).ToList().First();

            // Create questions for session
            List<SessionQuestion> sessionQuestions = new List<SessionQuestion>();
            for (int i = 0; i < questions.Count; i++)
            {
                // Get question
                Question question = questions[i];

                // Create session question
                SessionQuestion sessionQuestion = new SessionQuestion();
                sessionQuestion.Idsession = newSession.Idsession;
                sessionQuestion.Idquestion = question.Idquestion;
                sessionQuestion.OrderIndex = i;

                // Add to list
                sessionQuestions.Add(sessionQuestion);
            }

            // Add new session questions
            foreach (SessionQuestion sessionQuestion in sessionQuestions)
            {
                dbSessionQuestions.Add(sessionQuestion);
            }

            // Save again
            context.SaveChanges();

            // Return
            return Ok(mySession);
        }

        [HttpGet] // attribute
        [Route("question")]
        public IActionResult GetQuestion(int _sessionID)
        {
            // Get session
            Session dbSession = context.Sessions.Where(session => session.Idsession == _sessionID).FirstOrDefault();

            // Get question
            SessionQuestion sessionQuestion = context.SessionQuestions
                .Where(q => q.Idsession == dbSession.Idsession)
                .Where(q => q.OrderIndex == dbSession.CurrentQuestionIndex)
                .ToList().FirstOrDefault();

            // Return question
            return Ok(sessionQuestion);
        }

        [HttpPut]
        [Route("answers")]
        public IActionResult PostAnswer(int _sessionID, string _answerCharacter)
        {
            // Get session
            Session dbSession = context.Sessions.Where(session => session.Idsession == _sessionID).FirstOrDefault();

            // Get session question
            SessionQuestion dbSessionQuestion = context.SessionQuestions
                .Where(q => q.Idsession == dbSession.Idsession)
                .Where(q => q.OrderIndex == dbSession.CurrentQuestionIndex)
                .FirstOrDefault();

            // Get question
            Question dbQuestion = context.Questions.Where(q => q.Idquestion == dbSessionQuestion.Idquestion).FirstOrDefault();

            // Increase score
            if (dbQuestion.CorrectAnswer == _answerCharacter)
            {
                dbSession.Score++;
            }

            // Increment question
            dbSession.CurrentQuestionIndex++;

            // Save changes
            context.SaveChanges();

            // Return
            return Ok(dbSession);
        }

        [HttpPatch] // attribute
        [Route("score")]
        public IActionResult UpdateHighScore(int _idsession)
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
                    if (answer.Idsession == _idsession &&
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
                if (session.Idsession == _idsession)
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
        public IActionResult UpdateHighScoresAlternate(int _idsession) // Has more techniques
        {
            // Get tables
            var dbAnswers = context.Answers.Where(a => a.Idsession == _idsession).Select(a => new AnswerWithQuestion(a.AnswerCharacter, a.Idquestion)).ToList();

            var dbSessions = context.Sessions.FirstOrDefault(s => s.Idsession == _idsession);
            var sessionOrder = context.Sessions.OrderBy(s => s.Idsession);

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
            var sessionIds = context.Sessions.ToList();
            return Ok(sessionIds);
        }
    }

    // making a model
    public record CategoryWithQuestion(Category Categories, int QuestionCount);
    public record AnswerWithQuestion(string AnswerCharacter, int? IdQuestion); 
}
