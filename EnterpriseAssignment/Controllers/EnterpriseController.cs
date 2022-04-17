
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
        private readonly trivia_dbContext context;
    
        public EnterpriseController(trivia_dbContext context)
        {
            this.context = context;
        }

        //[HttpGet] // attribute
        //[Route("categories")]
        //public IActionResult GetCategories()
        //{
        //    var categories = context.Categories.Include(c => c.QuestionList);
        //    var categoryList = new List<CategoryWithQuestion>();
        //    foreach (var category in categories)
        //    {
        //        categoryList.Add(new CategoryWithQuestion(category, category.QuestionList.Count));
        //    }
        //    return Ok(categoryList);
        //}

        [HttpGet] // attribute
        [Route("categories")]
        public IActionResult GetCategories()
        {
            var categories = context.Categories;
            var categoryList = new List<Category>();
            foreach (var category in categories)
            {
                categoryList.Add(category);
            }
            return Ok(categoryList);
        }

        [HttpPut]
        [Route("session")]
        public IActionResult CreateSession(string _username, int _categoryId, int _questionCount, string _appId)
        {
            // Get database table
            var dbSessions = context.Sessions;
            var questions = context.Questions.Where(q => q.IdCategory == _categoryId).ToList();
            var dbSessionQuestions = context.Sessionquestions;
    
            // Get answer from http
            Session mySession = new Session();
            mySession.Name = _username;
            mySession.Score = 0;
            mySession.IdCategory = _categoryId;
            mySession.CurrentQuestionIndex = 0;
            mySession.QuestionCount = _questionCount;
            mySession.AppId = _appId;

            // Add to table
            dbSessions.Add(mySession);
    
            // Save changes
            context.SaveChanges();
    
            // Get session again now that it has a userId
            Session newSession = context.Sessions.Where(s => s.Name == _username).ToList().First();
    
            // Create questions for session
            List<Sessionquestion> sessionQuestions = new List<Sessionquestion>();
            float questionCount = MathF.Min(questions.Count, _questionCount);
            for (int i = 0; i < questionCount; i++)
            {
                // Get question
                Question question = questions[i];

                // Create session question
                Sessionquestion sessionQuestion = new Sessionquestion();
                sessionQuestion.IdSession = newSession.IdSession;
                sessionQuestion.IdQuestion = question.IdQuestion;
                sessionQuestion.OrderIndex = i;
    
                // Add to list
                sessionQuestions.Add(sessionQuestion);
            }
    
            // Add new session questions
            foreach (Sessionquestion sessionQuestion in sessionQuestions)
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
            Session dbSession = context.Sessions.Where(session => session.IdSession == _sessionID).FirstOrDefault();

            // Return immediately if session is over
            if (dbSession.EndSession == true)
            {
                return Ok("Session has ended. Your score is " + dbSession.Score);
            }

            // Get session question
            Sessionquestion sessionQuestion = context.Sessionquestions
                .Where(q => q.IdSession == dbSession.IdSession)
                .Where(q => q.OrderIndex == dbSession.CurrentQuestionIndex)
                .ToList().FirstOrDefault();

            // Get question
            Question question = context.Questions.Where(question => question.IdQuestion == sessionQuestion.IdQuestion).Single();

            // Get question text
            string text = question.Prompt + "\n A) " + question.AnswerA + "\n B) " + question.AnswerB;

            // Return question
            return Ok(text);
        }
    
        [HttpPut]
        [Route("answers")]
        public IActionResult PostAnswer(int _sessionID, string _answerCharacter)
        {
            // Get session
            Session dbSession = context.Sessions.Where(session => session.IdSession == _sessionID).FirstOrDefault();

            // Return immediately if session is over
            if (dbSession.EndSession == true)
            {
                return Ok("Session has ended. Your score is " + dbSession.Score);
            }

            // Get session question
            Sessionquestion dbSessionQuestion = context.Sessionquestions
                .Where(q => q.IdSession == dbSession.IdSession)
                .Where(q => q.OrderIndex == dbSession.CurrentQuestionIndex)
                .FirstOrDefault();
    
            // Get question
            Question dbQuestion = context.Questions.Where(q => q.IdQuestion == dbSessionQuestion.IdQuestion).FirstOrDefault();

            // get the location
            bool inRange = false;
            if (dbSession.Latitude  >= dbQuestion.Latitude  - 10 && dbSession.Latitude  <= dbQuestion.Latitude  + 10 &&
                dbSession.Longitude >= dbQuestion.Longitude - 10 && dbSession.Longitude <= dbQuestion.Longitude + 10)
            {
                inRange = true;
            }

            // Pass
            if (dbQuestion.CorrectAnswer == "PASS")
            {
                // move to next question 
                dbSession.CurrentQuestionIndex++;
                context.SaveChanges();
                return Ok("You have passed on this question");
            }

            // Location fail
            if (dbQuestion.hasLocation == true && inRange == false)
            {
                return Ok("You are in the wrong location");
            }

            // Correct/incorrect answer
            if (dbQuestion.CorrectAnswer == _answerCharacter)
            {
                // Get score to add
                int scoreToAdd = 2;
                if (dbSessionQuestion.TextHintUsed == true) { scoreToAdd--; }
                if (dbSessionQuestion.LocationHintUsed == true) { scoreToAdd--; }

                // Add score
                dbSession.Score += scoreToAdd;

                // Next question
                dbSession.CurrentQuestionIndex++;
                context.SaveChanges();

                // Return
                if (dbSession.CurrentQuestionIndex >= dbSession.QuestionCount)
                {
                    dbSession.EndSession = true;
                    context.SaveChanges();
                    return Ok("You answered correctly. Quiz now finished. Final score: " + dbSession.Score);
                }
                else
                {
                    return Ok("You answered correctly. Score now: " + dbSession.Score);
                }
            }
            else
            {
                return Ok("You answered incorrectly");
            }
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
                    if (answer.IdSession == _idsession &&
                        answer.IdQuestion == question.IdQuestion &&
                        answer.IdCategory == question.IdCategory &&
                        answer.AnswerString == question.CorrectAnswer)
                    {
                        score++;
                    }
                }
            }
    
            // Update session score
            foreach (Session session in dbSessions)
            {
                if (session.IdSession == _idsession)
                {
                    session.Score = score;
                }
            }
    
            // Save
            context.SaveChanges();
    
            // Return
            return Ok(score);
        }

        [HttpPatch] // attribute
        [Route("updateLocation")]
        public IActionResult UpdateLocation(int _latitude, int _longitude, int sessionId)
        {
            // Get tables
            Session dbSession = context.Sessions.Where(session => session.IdSession == sessionId).Single();
            dbSession.Latitude = _latitude;
            dbSession.Longitude = _longitude;
            context.SaveChanges();
            return Ok();
        }

        [HttpGet] // attribute
        [Route("hint")]
        public IActionResult GetHint(int _idSession)
        {
            Session dbSession = context.Sessions.Where(session => session.IdSession == _idSession).Single();

            // Return immediately if session is over
            if (dbSession.EndSession == true)
            {
                return Ok("Session has ended. Your score is " + dbSession.Score);
            }

            Sessionquestion sessionQuestion = context.Sessionquestions
                .Where(q => q.IdSession == dbSession.IdSession)
                .Where(q => q.OrderIndex == dbSession.CurrentQuestionIndex)
                .ToList().FirstOrDefault();

            Question question = context.Questions.Where(question => question.IdQuestion == sessionQuestion.IdQuestion).Single();

            string hint = question.Hint;

            sessionQuestion.TextHintUsed = true;
            
            context.SaveChanges();
            return Ok(hint);
        }

        [HttpGet] // attribute
        [Route("locationHint")]
        public IActionResult GetLocationHint(int _idSession)
        {
            Session dbSession = context.Sessions.Where(session => session.IdSession == _idSession).Single();

            // Return immediately if session is over
            if (dbSession.EndSession == true)
            {
                return Ok("Session has ended. Your score is " + dbSession.Score);
            }

            Sessionquestion sessionQuestion = context.Sessionquestions
                .Where(q => q.IdSession == dbSession.IdSession)
                .Where(q => q.OrderIndex == dbSession.CurrentQuestionIndex)
                .ToList().FirstOrDefault();

            Question question = context.Questions.Where(question => question.IdQuestion == sessionQuestion.IdQuestion).Single();

            float latitude  = question.Latitude;
            float longitude = question.Latitude;

            sessionQuestion.LocationHintUsed = true;

            context.SaveChanges();
            return Ok("Latitude: " + latitude + ", longitude: " + longitude);
        }

        [HttpGet] // attribute
        [Route("highscores")]
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
            var dbAnswers = context.Answers.Where(a => a.IdSession == _idsession).Select(a => new AnswerWithQuestion(a.AnswerString, a.IdQuestion)).ToList();
    
            var dbSessions = context.Sessions.FirstOrDefault(s => s.IdSession == _idsession);
            var sessionOrder = context.Sessions.OrderBy(s => s.IdSession);
    
            // Compare and increment score
            int score = 0;
            foreach (var answer in dbAnswers)
            {
                var question = context.Questions.FirstOrDefault(q => q.IdQuestion == answer.IdQuestion);
    
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

        [HttpGet]
        [Route("score")]
        public IActionResult GetSessionScore(int _idSession)
        {
            Session dbSession = context.Sessions.Where(session => session.IdSession == _idSession).Single();
            int score = dbSession.Score;
            return Ok(score);
        }
    }
    
    // making a model
    public record CategoryWithQuestion(Category Categories, int QuestionCount);
    public record AnswerWithQuestion(string AnswerCharacter, int? IdQuestion); 
}
