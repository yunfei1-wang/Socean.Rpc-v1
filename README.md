# Socean.Rpc
 
简介：
Socean.RPC是一个高效的rpc框架，框架特点是稳定和高效，在普通PC上测试，长连接模式下每秒处理请求数14w左右，常规请求处理时间0.04毫秒

本框架性能可满足部分unity3d或IM服务器端的需求，普通电脑上测试，可支持10000长连接，且每秒10个请求（每100毫秒1个请求 ），如果在性能好的服务器上，20000长连接且每秒20个请求应该是没问题的
  
框架特点:
高性能、稳定、支持异步、资源占用很小
  
  -------------------------------------------------------------------
  server sample :

  1.定义实体
  
    public class Book
    {
        public string Name { get; set; }
    }
 
 
 
  定义MessageProcessor
 
     public class DefaultMessageProcessor : IMessageProcessor
     {
          public void Init()
          {          
          }

          public async Task<ResponseBase> Process(Socean.Rpc.Core.Message.FrameData frameData)
          {
              var title = Encoding.UTF8.GetString(frameData.TitleBytes);
              if (title == "/books/namechange")
              {
                  var content = Encoding.UTF8.GetString(frameData.ContentBytes);

                  //here we use newtonsoft.Json serializer 
                  //you need add refer "newtonsoft.Json.dll"
                  var book = JsonConvert.DeserializeObject<Book>(content);
                  book.Name = "new name";

                  var responseContent = JsonConvert.SerializeObject(book);
                  return new BytesResponse(Encoding.UTF8.GetBytes(responseContent));
              }

              if (title == "test return empty")
              {
                  return new EmptyResponse();
              }

              return new ErrorResponse((byte)ResponseCode.SERVICE_NOT_FOUND);
          }
      }


  2.启动服务
  
    var server = new RpcServer();
    server.Bind(IPAddress.Any, 11111);       

    server.Start<DefaultMessageProcessor>();  
  
  -------------------------------------------------------------------

  client sample:
  
  1.定义实体
  
    public class Book
    {
        public string Name { get; set; }
    }
 
 
  2.执行同步调用
  
    public Book ChangeBookName(Book book)
    {
        using (var rpcClient = new FastRpcClient(IPAddress.Parse("127.0.0.1"), 11111))
        {
            var requestContent = JsonConvert.SerializeObject(book);
            var response = rpcClient.Query(Encoding.UTF8.GetBytes("/books/namechange"), Encoding.UTF8.GetBytes(requestContent));
            var content = Encoding.UTF8.GetString(response.ContentBytes);
            return JsonConvert.DeserializeObject<Book>(content);
        }
    }
    
  3.执行异步调用
  
    public async Task<Book> ChangeBookName(Book book)
    {
        using (var rpcClient = new FastRpcClient(IPAddress.Parse("127.0.0.1"), 11111))
        {
            var requestContent = JsonConvert.SerializeObject(book);
            var response = await rpcClient.QueryAsync(Encoding.UTF8.GetBytes("/books/namechange"), Encoding.UTF8.GetBytes(requestContent));
            var content = Encoding.UTF8.GetString(response.ContentBytes);
            return JsonConvert.DeserializeObject<Book>(content);
        }
    }
    
  -------------------------------------------------------------------
  
  其他：
     
  1.NetworkSettings类可修改连接超时时间等参数  
  
  
