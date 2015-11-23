using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private ArticlesModel articles = new ArticlesModel
        {
            Articles = new[]
            {
                new Article
                {
                    Title = "Fast and simple way to render shadows",
                    Content = @"The basic shadowmap algorithm consists in two passes. First, the scene is rendered from the point of view of the light. Only the depth of each fragment is computed. Next, the scene is rendered as usual, but with an extra test to see it the current fragment is in the shadow.
The “being in the shadow” test is actually quite simple. If the current sample is further from the light than the shadowmap at the same point, this means that the scene contains an object that is closer to the light. In other words, the current fragment is in the shadow.",
                    ImageSrc = "http://www.opengl-tutorial.org/wp-content/uploads/2011/08/shadowmapping.png"
                },
                new Article
                {
                    Title = "A few words about motion blur",
                    ImageSrc = "http://1.bp.blogspot.com/-e0R__QdylVQ/UQT8xw2QpFI/AAAAAAAAAbI/Ya5HjId0xCg/s1600/fig4.jpg",
                    Content = "Motion pictures are made up of a series of still images displayed in quick succession. Each image is captured by briefly opening a shutter to expose a piece of film/electronic sensor. If an object in the scene (or the camera itself) moves during this exposure, the result is blurred along the direction of motion, hence motion blur."
                },
                new Article
                {
                    Content = string.Empty,
                    ImageSrc = "https://cloud.githubusercontent.com/assets/5417867/7638478/98d376e4-fa8e-11e4-9aae-04b189f11ed5.png",
                    Title = "Making your own deferred renderer from scratch"
                },
                new Article
                {
                    Content = "CMake can handle in-place and out-of-place builds, enabling several builds from the same source tree, and cross-compilation. The ability to build a directory tree outside the source tree is a key feature, ensuring that if a build directory is removed, the source files remain unaffected.",
                    ImageSrc = "http://www.cmake.org/opensourcelogos/cmake100.png",
                    Title = "Making a cross-platform C++ application with CMake"
                }
            },
            Pictures = new[]
            {
                new Picture
                {
                    Description = "Screen Space Ambient Occlusion Example",
                    ImageAlt = "SSAO",
                    ImageSrc = "https://electronicmeteor.files.wordpress.com/2011/12/ssao-raw.jpg"
                },
                new Picture
                {
                    Description = "Poisson Disc shadow sampling",
                    ImageAlt = "Shadow Mapping",
                    ImageSrc = "https://electronicmeteor.files.wordpress.com/2013/02/poisson-2.jpg"
                },
                new Picture
                {
                    Description = "SSAO Comparison",
                    ImageAlt = "SSAO",
                    ImageSrc = "http://2.bp.blogspot.com/--8MtpXosA8s/UO2WyCalFHI/AAAAAAAAAWg/ne9rsn5xHqw/s1600/fig1.jpg"
                }
            }
        };

        private GalleryModel gallery = new GalleryModel
        {
            MaxImages = 6,
            Images = new[]
            {
                "/Content/Gallery/Images/1.png",
                "/Content/Gallery/Images/2.png",
                "/Content/Gallery/Images/3.png",
                "/Content/Gallery/Images/4.png",
                "/Content/Gallery/Images/5.png",
                "/Content/Gallery/Images/6.png",
                "/Content/Gallery/Images/7.png"
            },
            ImagePreviews = new[]
            {
                "/Content/Gallery/Images/1_s.jpg",
                "/Content/Gallery/Images/2_s.jpg",
                "/Content/Gallery/Images/3_s.jpg",
                "/Content/Gallery/Images/4_s.jpg",
                "/Content/Gallery/Images/5_s.jpg",
                "/Content/Gallery/Images/6_s.jpg",
                "/Content/Gallery/Images/7_s.jpg"
            }
        };

        public ActionResult Index()
        {
            return View(articles);
        }

        public ActionResult Article(int index = 0)
        {
            return View(articles.Articles[index]);
        }

        public ActionResult Gallery()
        {
            return View(gallery);
        }

        public async Task<ActionResult> WebM()
        {
            var client = new WebClient();
            var data = await client.DownloadDataTaskAsync(new Uri("http://2ch.hk/b/catalog.json"));
            dynamic catalog = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data));
            dynamic[] threads = catalog.threads.ToObject<dynamic[]>();
            var webm = threads.First(x => (
                x.comment.ToObject<string>().IndexOf("WebM", StringComparison.OrdinalIgnoreCase) >= 0
                || x.comment.ToObject<string>().IndexOf("ЦуиЬ", StringComparison.OrdinalIgnoreCase) >= 0)
                && x.comment.ToObject<string>().IndexOf("Анимублядский", StringComparison.OrdinalIgnoreCase) < 0);
            var numThread = webm.num.ToObject<string>();
            var threadData = await client.DownloadDataTaskAsync(new Uri("http://2ch.hk/b/res/" + numThread + ".json"));
            dynamic thread = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(threadData));
            var posts = thread.threads.ToObject<dynamic[]>()[0].posts.ToObject<dynamic[]>();
            var webms = new List<string>();
            var previews = new List<string>();
            foreach (var post in posts)
            {
                foreach (var file in post.files.ToObject<dynamic[]>())
                {
                    var path = file.path.ToObject<string>();
                    var thumbnail = file.thumbnail.ToObject<string>();
                    if (path.EndsWith(".webm"))
                    {
                        webms.Add("http://2ch.hk/b/" + path);
                        previews.Add("http://2ch.hk/b/" + thumbnail);
                    }
                }
            }
            var model = new GalleryModel
            {
                MaxImages = 6,
                Images = webms.ToArray(),
                ImagePreviews = previews.ToArray()
            };
            return View(model);
        }

        public ActionResult Forum()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}