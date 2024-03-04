// Copyright (C) Abc Arbitrage Asset Management - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// Written by Olivier Coanet <o.coanet@abc-arbitrage.com>, 2020-10-01

using System.Collections.Generic;
using System.Linq;

namespace AbcArbitrage.Homework.Routing
{
    public class SubscriptionIndex : ISubscriptionIndex
    {
        private List<Subscription> _subscriptions = new();

        public void AddSubscriptions(IEnumerable<Subscription> subscriptions)
        {
            _subscriptions.AddRange(subscriptions);
        }

        public void RemoveSubscriptions(IEnumerable<Subscription> subscriptions)
        {
            foreach (var subscription in subscriptions)
            {
                _subscriptions.Remove(subscription);
            }
        }

        public void RemoveSubscriptionsForConsumer(ClientId consumer)
        {
            _subscriptions.RemoveAll(src => src.ConsumerId.Equals(consumer));
        }

        public IEnumerable<Subscription> FindSubscriptions(MessageTypeId messageTypeId,
            MessageRoutingContent routingContent)
        {
            var subscriptions = _subscriptions
                .Where(subscription => subscription.MessageTypeId.Equals(messageTypeId) &
                                       (routingContent.Parts == null ||
                                        subscription.ContentPattern.Parts.Any(part =>
                                            routingContent.Parts.Any(routingContentParts =>
                                                routingContentParts == "*" || routingContentParts == part ||
                                                ContentPattern.Split(routingContentParts)
                                                    .Parts
                                                    .Contains(part)
                                            )
                                        )
                                       )
                );

            return subscriptions;
        }
    }
}
