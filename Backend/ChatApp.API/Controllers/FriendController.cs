using ChatApp.Application.DTOs;
using ChatApp.Application.Model;
using ChatApp.Application.UseCases.Friend.Commands;
using ChatApp.Application.UseCases.Friend.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ChatApp.API.Controllers
{
    [Authorize]
    public class FriendController : BaseController
    {
        /// <summary>
        /// Khởi tạo <see cref="FriendController"/> với dependency injection của IMediator.
        /// </summary>
        /// <param name="mediator">Đối tượng IMediator để gửi các command/query.</param>
        public FriendController(IMediator mediator) : base(mediator) { }

        /// <summary>
        /// Gửi lời mời kết bạn đến người dùng khác.
        /// </summary>
        /// <param name="command">Thông tin lời mời kết bạn.</param>
        /// <returns>Kết quả thực thi (thành công/thất bại).</returns>
        [HttpPost("add")]
        public async Task<ActionResult<APIResponse>> AddFriend([FromBody] AddFriendCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Xóa bạn khỏi danh sách bạn bè.
        /// </summary>
        /// <param name="command">Thông tin người bạn cần xóa.</param>
        /// <returns>Kết quả thực thi (thành công/thất bại).</returns>
        [HttpPost("remove")]
        public async Task<ActionResult<APIResponse>> RemoveFriend([FromBody] RemoveFriendCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Chấp nhận lời mời kết bạn.
        /// </summary>
        /// <param name="command">Thông tin lời mời cần chấp nhận.</param>
        /// <returns>Kết quả thực thi (thành công/thất bại).</returns>
        [HttpPost("accept")]
        public async Task<ActionResult<APIResponse>> AcceptFriend([FromBody] AcceptFriendCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Từ chối lời mời kết bạn.
        /// </summary>
        /// <param name="command">Thông tin lời mời cần từ chối.</param>
        /// <returns>Kết quả thực thi (thành công/thất bại).</returns>
        [HttpPost("reject")]
        public async Task<ActionResult<APIResponse>> RejectFriend([FromBody] RejectFriendCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Lấy danh sách bạn bè của người dùng hiện tại.
        /// </summary>
        /// <returns>Danh sách bạn bè.</returns>
        [HttpGet("get-list")]
        public async Task<ActionResult<APIResponse<IEnumerable<FriendDTO>>>> GetFriendList()
        {
            var result = await _mediator.Send(new GetFriendListQuery());
            return Ok(result);
        }

        /// <summary>
        /// Lấy danh sách các lời mời kết bạn đến người dùng hiện tại.
        /// </summary>
        /// <returns>Danh sách lời mời kết bạn.</returns>
        [HttpGet("list-request")]
        public async Task<ActionResult<APIResponse<IEnumerable<FriendDTO>>>> GetFriendRequestList()
        {
            var result = await _mediator.Send(new GetListFriendRequestQuery());
            return Ok(result);
        }

        /// <summary>
        /// Chặn người dùng.
        /// </summary>
        /// <returns>Danh sách lời mời kết bạn.</returns>
        [HttpPost("block")]
        public async Task<ActionResult<APIResponse<IEnumerable<FriendDTO>>>> BlockFriend( [FromBody] BlockFriendCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Hủy chặn người dùng.
        /// </summary>
        /// <returns>Danh sách lời mời kết bạn.</returns>
        [HttpPost("unblock")]
        public async Task<ActionResult<APIResponse<IEnumerable<FriendDTO>>>> UnBlockFriend([FromBody] UnBlockFriendCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
